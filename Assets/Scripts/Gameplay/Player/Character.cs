using System.Collections.Generic;
using UnityEngine;
using Gameplay.Terrain;
using Gameplay.State;
using Gameplay.Data;

namespace Gameplay.Player
{

    public struct CharacterType
    {
        public Color SkinColor { get; }
        public Color EyeColor { get; }
        public Color IrisColor { get; }
        public Color PupilColor { get; }
        public float SkinRoughness { get; }
        public float EyelidPosition { get; }
        public float EyeSize { get; }
        public float EyeSpacing { get; }


        public CharacterType(bool param)
        {
            SkinColor = Random.ColorHSV(0, 1);
            EyeColor = Random.ColorHSV(0, 1);
            IrisColor = Random.ColorHSV(0, 1);
            PupilColor = Random.ColorHSV(0, 1);
            SkinRoughness = Random.Range(.01f, 1);
            EyelidPosition = Random.Range(.1f, .44f);
            EyeSize = Random.Range(.5f, 1);
            EyeSpacing = Random.Range(-.95f, -.5f);
        }
    }
    public class Character : MonoBehaviour
    {
        public struct Equipment
        {
            public Balloon balloon;
            public Innertube innertube;
            public Umbrella umbrella;
            public Gun gun;
            public Hammer hammer;
        }
        private float soundCount = 0;
        public VoiceSet voiceSet;
        public float voicePitch;
        public float speechCooldown = 2;
        public bool dead;

        public enum SpeechQueue {
            Attack,
            Hurt,
            Greeting,
            Idle,
        }
        public SpeechQueue speechQueue = SpeechQueue.Idle;

        private AudioSource voice;
        private AudioSource lavaPlayer;
        public AudioClip[] sandSound;
        public AudioClip[] waterSound;
        public AudioClip[] grassSound;
        public AudioClip[] dirtSound;
        public AudioClip lavaSound;
        public AudioClip[] quickSandSound;
        public AudioClip[] rockSound;
        public Equipment gear;
        public GameObject RightEye;
        public GameObject LeftEye;
        public GameObject Body;
        public float Health = 10;
        public float MaxHealth = 10;
        public float Speed = .2f;
        public float ModifiedSpeed = .2f;
        public Vector2 movement;
        public TerrainType terrainType;
        // public List<TerrainType> AllowedTerrain;
        public bool floating = false;
        public bool falling = false;
        public bool suffocating = false;
        public bool burning = false;
        public float targetFloatHeight = 0;
        public float verticalVelocity = 0;
        private float suffocationModifier = .4f;
        public AI ai;


        public Color SkinColor;
        public Color EyeColor;
        public Color IrisColor;
        public Color PupilColor;
        [Range(0, 1)]
        public float SkinRoughness;
        [Range(.1f, .44f)]
        public float EyelidPosition;
        [Range(0, 1)]
        public float EyeSize;
        [Range(0, 1)]
        public float EyeSpacing;
        [Range(.5f, 1)]
        public float BodyHeight;
        public float BodyWidth;
        [Range(.05f, .147f)]
        public float EyeLookY;
        [Range(-.27f, -.23f)]
        public float EyeLookX;
        [Range(.1f, 1)]
        public float IrisSize;
        [Range(.1f, 1)]
        public float PupilSize;
        public float Smile;
        public float MouthWidth;
        public float MouthOpen;
        public float TeethOpen;
        public float damage;



        public void MeleeAttack()
        {
            if (gear.hammer != null)
            {
                gear.hammer.Attack();
            }
        }

        public void RangeAttack()
        {
            if (gear.gun != null)
            {
                gear.gun.Attack();
            }
        }

        public void Jump()
        {

        }
        // private float seconds = 0;
        private void Update()
        {
            if (dead) {
                return;
            }
            // seconds += Time.deltaTime;
            // if (seconds >= 1) {
            //     Randomize();
            //     SetCustomizationValues();
            //     seconds = 0;
            // }
            if (ai != null)
            {
                CheckAttack();
                pickupStuff();
                cleanup();
                // do ai stuff
                speak();
            }
            walkSound();
        }

        private void FixedUpdate()
        {
            if (dead) {
                return;
            }
            if (ai != null)
            {
                MakeEquip();
                CheckGearModifiers();
                CheckTerrainModifiers();
                Float();
                ai.move(this);
                
            }
            if (suffocating) {
                Health -= suffocationModifier * Time.deltaTime;
            }
        }

        public void CheckAttack()
        {

            Weapon weapon = ai.ShouldAttack(this);
            if (weapon != null)
            {
                speechQueue = SpeechQueue.Attack;
                speechCooldown = 0;
                weapon.Attack();
            }
        }

        public void pickupStuff()
        {
            GameObject[] allGear = GameObject.FindGameObjectsWithTag("Gear");
            foreach (GameObject go in allGear)
            {
                if (Vector2.Distance(new Vector2(go.transform.position.x, go.transform.position.z), new Vector2(transform.position.x, transform.position.z)) > 2)
                {
                    continue;
                }
                Gear g = go.GetComponent<Gear>();
                if (g != null && g.equippedCharacter == null)
                {
                    // g.equippedCharacter = MainCharacter;
                    g.PickUp(this);
                }
            }
        }

        private void cleanup()
        {
            if (damage > 0)
            {
                Health -= damage;
            }
            damage = 0;
            if (Health < 0)
            {
                if (gear.balloon != null)
                {
                    gear.balloon.Drop();
                }
                if (gear.innertube != null)
                {
                    gear.innertube.Drop();
                }
                if (gear.umbrella != null)
                {
                    gear.umbrella.Drop();
                }
                if (gear.hammer != null)
                {
                    gear.hammer.Drop();
                }
                if (gear.gun != null)
                {
                    gear.gun.Drop();
                }
                // Destroy(gameObject);
                die();
                return;
            }
            Ray ray = new Ray(new Vector3(transform.position.x, 0.5f, transform.position.z), Vector3.down);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit))
            {
                // Destroy(gameObject);
                die();
            }
        }

        public void die() {
            ai.mainCharacter = null;
            ai = null;
            Body.transform.parent = null;
            LeftEye.transform.parent = null;
            RightEye.transform.parent = null;
            Body.AddComponent<Rigidbody>();
            LeftEye.AddComponent<Rigidbody>();
            RightEye.AddComponent<Rigidbody>();
            dead = true;
            Destroy(gameObject);
            Destroy(Body, 10);
            Destroy(LeftEye, 10);
            Destroy(RightEye, 10);
        }

        // private void Awake()
        // {
        //     Randomize();
        // }
        // private void OnDrawGizmos() {
        //     if (ai != null)
        //     {
        //         Gizmos.color = Color.red;
        //         Gizmos.DrawSphere(transform.position, ai.attackRadius);
        //     }
        // }
        public void Randomize()
        {
            // type traits
            SkinColor = Random.ColorHSV(0, 1);
            EyeColor = Random.ColorHSV(0, 1);
            IrisColor = Random.ColorHSV(0, 1);
            PupilColor = Random.ColorHSV(0, 1);
            SkinRoughness = Random.Range(.01f, 1);
            EyelidPosition = Random.Range(.1f, .44f);
            EyeSize = Random.Range(.5f, 1);
            EyeSpacing = Random.Range(-.95f, -.5f);
            // Individual traits
            BodyHeight = Random.Range(.5f, 1);
            BodyWidth = Random.Range(.5f, 1);
            IrisSize = Random.Range(.08f, .16f);
            PupilSize = Random.Range(0.01f, .08f);
            MouthOpen = Random.Range(.03f, .232f);
            MouthWidth = Random.Range(.023f, .238f);
            TeethOpen = Random.Range(0, .238f);
            Smile = Random.Range(-0.1f, .671f);


            EyeLookY = Random.Range(.05f, .147f);
            EyeLookX = -.2498f;
            // EyeLookX = Random.Range(-.27f, -.23f);
        }

        public void Randomize(CharacterType type)
        {
            // type traits
            SkinColor = type.SkinColor;
            EyeColor = type.EyeColor;
            IrisColor = type.IrisColor;
            PupilColor = type.PupilColor;
            SkinRoughness = type.SkinRoughness;
            EyelidPosition = type.EyelidPosition;
            EyeSize = type.EyeSize;
            EyeSpacing = type.EyeSpacing;
            // individual traits
            BodyHeight = Random.Range(.5f, 1);
            BodyWidth = Random.Range(.5f, 1);
            IrisSize = Random.Range(.08f, .16f);
            PupilSize = Random.Range(0.01f, .08f);
            MouthOpen = Random.Range(.03f, .232f);
            MouthWidth = Random.Range(.023f, .238f);
            TeethOpen = Random.Range(0, .238f);
            Smile = Random.Range(-0.1f, .671f);


            EyeLookY = Random.Range(.05f, .147f);
            EyeLookX = -.2498f;
        }

        public void SetCustomizationValues()
        {
            Material _body = Body.gameObject.GetComponent<MeshRenderer>().material;
            Material _rightEye = RightEye.gameObject.GetComponent<MeshRenderer>().material;
            Material _leftEye = LeftEye.gameObject.GetComponent<MeshRenderer>().material;
            // SkinColor
            _body.SetColor("Color_c9b0e0dfacb84c87a24618eea7b3d861", SkinColor);
            _rightEye.SetColor("Color_8cbb982647ed49f0a5c9f595711113f0", SkinColor);
            _leftEye.SetColor("Color_8cbb982647ed49f0a5c9f595711113f0", SkinColor);
            // SkinRoughness
            _body.SetFloat("Vector1_84b477cc3c2e4d089342a26b2a53ab9e", SkinRoughness);
            _rightEye.SetFloat("Vector1_549f0fee6d3c40239cb492f1c0dfe4df", SkinRoughness);
            _leftEye.SetFloat("Vector1_549f0fee6d3c40239cb492f1c0dfe4df", SkinRoughness);
            // EyelipPosition / eyeOpen
            _rightEye.SetFloat("Vector1_4f08c76a1404422a8d66ae996bd2fbfa", EyelidPosition);
            _leftEye.SetFloat("Vector1_4f08c76a1404422a8d66ae996bd2fbfa", EyelidPosition);
            // EyeColor
            _rightEye.SetColor("Color_ca45d8a7905f40c291a1451df70014b0", EyeColor);
            _leftEye.SetColor("Color_ca45d8a7905f40c291a1451df70014b0", EyeColor);
            // IrisColor
            _rightEye.SetColor("Color_ac1f9112475c432685728afe941b7661", IrisColor);
            _leftEye.SetColor("Color_ac1f9112475c432685728afe941b7661", IrisColor);
            // PupilColor
            _rightEye.SetColor("Color_c0e17f4523ff42719aacaa10999c39c9", PupilColor);
            _leftEye.SetColor("Color_c0e17f4523ff42719aacaa10999c39c9", PupilColor);
            // IrisSize
            _rightEye.SetFloat("Vector1_eae4e3df392e4e06844152a91f4b5887", IrisSize);
            _leftEye.SetFloat("Vector1_eae4e3df392e4e06844152a91f4b5887", IrisSize);
            // PupilSize
            _rightEye.SetFloat("Vector1_572ab91db3d54d6c95239de619484396", PupilSize);
            _leftEye.SetFloat("Vector1_572ab91db3d54d6c95239de619484396", PupilSize);
            // LookX
            _rightEye.SetFloat("Vector1_9979475be7dd4d029c53f3b8d0bbb64a", EyeLookX);
            _leftEye.SetFloat("Vector1_9979475be7dd4d029c53f3b8d0bbb64a", EyeLookX);
            // LookY
            _rightEye.SetFloat("Vector1_bcb7e772ac2543f0a4f4ee289502ad17", EyeLookY);
            _leftEye.SetFloat("Vector1_bcb7e772ac2543f0a4f4ee289502ad17", EyeLookY);
            // Mouth
            _body.SetFloat("Vector1_6decf4ff65b849a3a10735c6af22a86c", MouthOpen);
            _body.SetFloat("Vector1_9785ac54f99345cdac2f47ee51317a62", MouthWidth);
            _body.SetFloat("Vector1_fe2e7edb90364604929ec17303e41edb", Smile);
            _body.SetFloat("Vector1_a57ded8bb1e042dfbb2e2f2ab0e0bc3f", TeethOpen);
            // BodyHeight
            Body.gameObject.transform.localScale = new Vector3(
                BodyWidth,
                BodyHeight,
                BodyWidth
            );
            // EyeSize
            RightEye.gameObject.transform.localScale = new Vector3(
                EyeSize,
                EyeSize,
                EyeSize
            );
            LeftEye.gameObject.transform.localScale = new Vector3(
                EyeSize,
                EyeSize,
                EyeSize
            );
            // EyeSpacing
            Vector3 directionOfTravelRight = -Body.transform.right;
            Vector3 rightEyeDirection = directionOfTravelRight + directionOfTravelRight.normalized * EyeSpacing;
            RightEye.transform.position = Body.transform.position + rightEyeDirection;

            Vector3 directionOfTravelLeft = Body.transform.right;
            Vector3 leftEyeDirection = directionOfTravelLeft + directionOfTravelLeft.normalized * EyeSpacing;
            LeftEye.transform.position = Body.transform.position + leftEyeDirection;
             // MainCharacter.Randomize();
            // Material _body = Body.GetComponent<MeshRenderer>().material;
            // Material _rightEye = RightEye.GetComponent<MeshRenderer>().material;
            // Material _leftEye = LeftEye.GetComponent<MeshRenderer>().material;
            // // SkinColor
            // _body.SetColor("Color_c9b0e0dfacb84c87a24618eea7b3d861", GameSettings.bodyColor);
            // _rightEye.SetColor("Color_8cbb982647ed49f0a5c9f595711113f0", GameSettings.bodyColor);
            // _leftEye.SetColor("Color_8cbb982647ed49f0a5c9f595711113f0", GameSettings.bodyColor);
            // // SkinRoughness
            // _body.SetFloat("Vector1_84b477cc3c2e4d089342a26b2a53ab9e", GameSettings.roughness);
            // _rightEye.SetFloat("Vector1_549f0fee6d3c40239cb492f1c0dfe4df", GameSettings.roughness);
            // _leftEye.SetFloat("Vector1_549f0fee6d3c40239cb492f1c0dfe4df", GameSettings.roughness);
            // // EyeColor
            // _rightEye.SetColor("Color_ca45d8a7905f40c291a1451df70014b0", GameSettings.eyeColor);
            // _leftEye.SetColor("Color_ca45d8a7905f40c291a1451df70014b0", GameSettings.eyeColor);
            // // Eye Open
            // _rightEye.SetFloat("Vector1_4f08c76a1404422a8d66ae996bd2fbfa", GameSettings.eyeOpen);
            // _leftEye.SetFloat("Vector1_4f08c76a1404422a8d66ae996bd2fbfa", GameSettings.eyeOpen);
            // // IrisColor
            // _rightEye.SetColor("Color_ac1f9112475c432685728afe941b7661", GameSettings.irisColor);
            // _leftEye.SetColor("Color_ac1f9112475c432685728afe941b7661", GameSettings.irisColor);
            // // PupilColor
            // _rightEye.SetColor("Color_c0e17f4523ff42719aacaa10999c39c9", GameSettings.pupilColor);
            // _leftEye.SetColor("Color_c0e17f4523ff42719aacaa10999c39c9", GameSettings.pupilColor);
            // // IrisSize
            // _rightEye.SetFloat("Vector1_eae4e3df392e4e06844152a91f4b5887", GameSettings.irisSize);
            // _leftEye.SetFloat("Vector1_eae4e3df392e4e06844152a91f4b5887", GameSettings.irisSize);
            // // PupilSize
            // _rightEye.SetFloat("Vector1_572ab91db3d54d6c95239de619484396", GameSettings.pupilSize);
            // _leftEye.SetFloat("Vector1_572ab91db3d54d6c95239de619484396", GameSettings.pupilSize);
            // // LookX
            // _rightEye.SetFloat("Vector1_9979475be7dd4d029c53f3b8d0bbb64a", GameSettings.eyeX);
            // _leftEye.SetFloat("Vector1_9979475be7dd4d029c53f3b8d0bbb64a", GameSettings.eyeX);
            // // LookY
            // _rightEye.SetFloat("Vector1_bcb7e772ac2543f0a4f4ee289502ad17", GameSettings.eyeY);
            // _leftEye.SetFloat("Vector1_bcb7e772ac2543f0a4f4ee289502ad17", GameSettings.eyeY);
            // // Mouth
            // _body.SetFloat("Vector1_6decf4ff65b849a3a10735c6af22a86c", GameSettings.mouthOpen);
            // _body.SetFloat("Vector1_9785ac54f99345cdac2f47ee51317a62", GameSettings.mouthWidth);
            // _body.SetFloat("Vector1_fe2e7edb90364604929ec17303e41edb", GameSettings.smile);
            // _body.SetFloat("Vector1_a57ded8bb1e042dfbb2e2f2ab0e0bc3f", GameSettings.teethOpen);
            // // BodyHeight
            // Body.gameObject.transform.localScale = new Vector3(
            //     GameSettings.bodyWidth,
            //     GameSettings.bodyHeight,
            //     GameSettings.bodyWidth
            // );
            // // EyeSize
            // RightEye.gameObject.transform.localScale = new Vector3(
            //     GameSettings.eyeSize,
            //     GameSettings.eyeSize,
            //     GameSettings.eyeSize
            // );
            // LeftEye.gameObject.transform.localScale = new Vector3(
            //     GameSettings.eyeSize,
            //     GameSettings.eyeSize,
            //     GameSettings.eyeSize
            // );
            // // EyeSpacing
            // Vector3 directionOfTravelRight =  Body.transform.right;
            // Vector3 rightEyeDirection = directionOfTravelRight + directionOfTravelRight.normalized * GameSettings.eyeSpacing;
            // RightEye.transform.position = Body.transform.position + rightEyeDirection;

            // Vector3 directionOfTravelLeft = Body.transform.right;
            // Vector3 leftEyeDirection = directionOfTravelLeft + directionOfTravelLeft.normalized * GameSettings.eyeSpacing;
            // LeftEye.transform.position = Body.transform.position + leftEyeDirection;
        }

        public void CheckGearModifiers()
        {
            floating = false;
            ModifiedSpeed = Speed;
            targetFloatHeight = 0.5f;
            // foreach (Gear g in gear)
            // {
            //     g.MoveModifier();
            // }
            if (gear.balloon != null)
            {
                gear.balloon.MoveModifier();
            }
            if (gear.innertube != null)
            {
                gear.innertube.MoveModifier();
            }
            if (gear.umbrella != null)
            {
                gear.umbrella.MoveModifier();
            }
            if (gear.hammer != null)
            {
                gear.hammer.MoveModifier();
            }
            if (gear.gun != null)
            {
                gear.gun.MoveModifier();
            }

        }

        // public void Unequip(Gear g)
        // {

        //     // gear.Remove(g);
        //     switch(g.GetType()) {
        //         case typeof(Balloon):
        //             gear.balloon = null;
        //             break;
        //     }
        // }

        public void MakeEquip()
        {
            // foreach (Gear g in gear)
            // {
            //     g.makeEquip();
            // }
            if (gear.balloon != null)
            {
                gear.balloon.makeEquip();
                if (gear.balloon.health <= 0)
                {
                    Destroy(gear.balloon.gameObject);
                    gear.balloon = null;
                }
            }
            if (gear.innertube != null)
            {
                gear.innertube.makeEquip();
                if (gear.innertube.health <= 0)
                {
                    Destroy(gear.innertube.gameObject);
                    gear.innertube = null;
                }
            }
            if (gear.umbrella != null)
            {
                gear.umbrella.makeEquip();
                if (gear.umbrella.health <= 0)
                {
                    Destroy(gear.umbrella.gameObject);
                    gear.umbrella = null;
                }
            }
            if (gear.hammer != null)
            {
                gear.hammer.makeEquip();
                if (gear.hammer.health <= 0)
                {
                    Destroy(gear.hammer.gameObject);
                    gear.hammer = null;
                }
            }
            if (gear.gun != null)
            {
                gear.gun.makeEquip();
                if (gear.gun.health <= 0)
                {
                    Destroy(gear.gun.gameObject);
                    gear.gun = null;
                }
            }

            // int i = 0;

            // while (i < gear.Count)
            // {
            //     Gear g = gear[i];
            //     if (g.health < 0)
            //     {
            //         g.Unequip();
            //         Unequip(g);
            //         Destroy(g);
            //         Destroy(g.gameObject);
            //         i = 0;
            //     }
            //     else
            //     {
            //         i++;
            //     }
            // }
        }

        public void Float()
        {
            float height = transform.position.y;
            if (floating)
            {
                float verticalOffset = (Mathf.Sin(Time.timeSinceLevelLoad * 5) * 0.1f) + targetFloatHeight;
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, verticalOffset, transform.position.z), Time.deltaTime * 5);
            }
            else if (transform.position.y < 0.5f && !falling)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0.5f, transform.position.z), Time.deltaTime * 50);
            }
            else if (transform.position.y > 0.5f && !falling)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0.5f, transform.position.z), Time.deltaTime * 5);
            }
        }

        public void SetTerrainType()
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                int index = hit.triangleIndex * 3;
                MeshCollider mc = hit.collider as MeshCollider;
                if (mc == null)
                {
                    return;
                }
                Mesh mesh = mc.sharedMesh;
                Vector2 uv2 = mesh.uv2[mesh.triangles[index]];
                terrainType = (TerrainType)(uv2.x);
            }
        }

        public void walkSound()
        {

            if (!floating || (terrainType == TerrainType.Water && transform.position.y <= 0))
            {
                switch (terrainType)
                {
                    case TerrainType.Sand:
                        if (soundCount < ModifiedSpeed * 3)
                        {
                            soundCount += Time.deltaTime;
                        }
                        else
                        {
                            playSound(sandSound, 1, 1.1f);
                            soundCount = 0;
                        }
                        break;
                    case TerrainType.Dirt:
                        if (soundCount < ModifiedSpeed * 3)
                        {
                            soundCount += Time.deltaTime;
                        }
                        else
                        {
                            playSound(dirtSound, 1, 1.1f);
                            soundCount = 0;
                        }
                        break;
                    case TerrainType.Grass:
                        if (soundCount < ModifiedSpeed * 3)
                        {
                            soundCount += Time.deltaTime;
                        }
                        else
                        {
                            playSound(grassSound, 1, 1.1f);
                            soundCount = 0;
                        }
                        break;
                    case TerrainType.Water:
                        if (soundCount < ModifiedSpeed * 10)
                        {
                            soundCount += Time.deltaTime;
                        }
                        else
                        {
                            playSound(waterSound, 1, 1.1f);
                            soundCount = 0;
                        }
                        break;
                    case TerrainType.Rock:
                        if (soundCount < ModifiedSpeed * 3)
                        {
                            soundCount += Time.deltaTime;
                        }
                        else
                        {
                            playSound(rockSound, 1, 1.1f);
                            soundCount = 0;
                        }
                        break;
                    case TerrainType.QuickSand:
                        if (soundCount < ModifiedSpeed * 3)
                        {
                            soundCount += Time.deltaTime;
                        }
                        else
                        {
                            playSound(quickSandSound, 1, 1.1f);
                            soundCount = 0;
                        }
                        break;
                    default:
                        break;
                }
                if (lavaPlayer == null)
                {
                    lavaPlayer = gameObject.AddComponent<AudioSource>();
                    lavaPlayer.loop = true;
                    lavaPlayer.spatialBlend = 1;
                    lavaPlayer.clip = lavaSound;
                }
                if (terrainType == TerrainType.Lava)
                {
                    if (!lavaPlayer.isPlaying)
                    {
                        lavaPlayer.Play();
                    }
                    
                }
                else if (lavaPlayer.isPlaying) 
                {
                    lavaPlayer.Stop();
                }
            }
        }

        public void TakeDamage(float score) {
            Health -= score;
            speechQueue = SpeechQueue.Hurt;
            speechCooldown = 0;
        }

        public void speak() {
            if (ai == null) {
                return;
            }
            if (voice == null) {
                voice = GetComponent<AudioSource>();
                voice.pitch = voicePitch;
                voice.volume = 1;
                voice.spatialBlend = 1;
            }
            if (voice.isPlaying) {
                if (voice.time > voice.clip.length - .2f) {
                    voice.volume -= Time.deltaTime * .02f;
                }
            }
            if (speechCooldown > 0)
            {
                speechCooldown -= Time.deltaTime;
                return;
            }
            if (voice.isPlaying) {
                speechCooldown = 2;
                return;
            }
            int mood = Mathf.FloorToInt(Mathf.Lerp(0, 2.99f, ai.mood));
            AudioClip[] set = voiceSet.GetSet(mood, speechQueue);
            AudioClip clip = set[Random.Range(0, set.Length)];
            voice.clip = clip;
            voice.Play();

            if (speechQueue == SpeechQueue.Idle) {
                speechQueue = SpeechQueue.Greeting;
            } else {
                speechQueue = SpeechQueue.Idle;             
            }
            voice.volume = .75f;
            speechCooldown = Random.Range(1f, 10f);
        }

        public void playSound(AudioClip[] clip, float pitchMin, float pitchMax)
        {
            AudioSource source = GetComponent<AudioSource>();
            source.clip = clip[Random.Range(0, clip.Length)];
            source.pitch = Random.Range(pitchMin, pitchMax);
            source.Play();
        }
        public void CheckTerrainModifiers()
        {
            falling = false;
            suffocating = false;
            burning = false;
            switch (terrainType)
            {
                case TerrainType.Grass:
                    break;
                case TerrainType.Dirt:
                    ModifiedSpeed *= .9f;
                    break;
                case TerrainType.Rock:
                    ModifiedSpeed *= 1.1f;
                    break;
                case TerrainType.Water:
                    if (transform.position.y <= .5f)
                    {
                        ModifiedSpeed *= .5f;
                    }
                    if (!floating)
                    {
                        falling = true;
                        transform.position = new Vector3(transform.position.x, transform.position.y - (Time.deltaTime * .5f), transform.position.z);
                        if (transform.position.y <= -.5f)
                        {
                            transform.position = new Vector3(transform.position.x, -.5f, transform.position.z);
                            // Health -= Time.deltaTime * suffocationModifier;
                            suffocating = true;
                        }
                    }
                    break;
                case TerrainType.Hole:
                    if (!floating)
                    {
                        falling = true;
                        transform.position = new Vector3(transform.position.x, transform.position.y - (Time.deltaTime * 3), transform.position.z);
                        if (transform.position.y < -2)
                        {
                            Health = 0;
                        }
                    }
                    break;
                case TerrainType.Sand:
                    if (!floating)
                    {
                        ModifiedSpeed *= .65f;
                    }
                    break;
                case TerrainType.QuickSand:
                    if (transform.position.y < .5f)
                    {
                        ModifiedSpeed *= Mathf.Clamp((transform.position.y + .5f), .2f, 1);
                    }
                    if (!floating)
                    {
                        falling = true;
                        transform.position = new Vector3(transform.position.x, transform.position.y - (Time.deltaTime * .1f), transform.position.z);
                        if (transform.position.y <= -.25f)
                        {
                            transform.position = new Vector3(transform.position.x, -.5f, transform.position.z);
                            suffocating = true;
                        }
                    }
                    break;
                case TerrainType.Lava:
                    // foreach (Gear g in gear)
                    // {
                    //     if (g.GetType() == typeof(Balloon))
                    //     {
                    //         Balloon b = g as Balloon;
                    //         b.health -= Time.deltaTime * 3;
                    //     }
                    // }
                    if (gear.balloon != null)
                    {
                        gear.balloon.health -= Time.deltaTime * 3;
                    }
                    if (!floating)
                    {
                        burning = true;
                        ModifiedSpeed *= Health / MaxHealth;
                        Health -= Time.deltaTime * 3;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
