using System.Collections.Generic;
using UnityEngine;
using Gameplay.Terrain;
using Gameplay.State;

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
        public bool suffocationg = false;
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




        // private float seconds = 0;
        private void Update()
        {
            // seconds += Time.deltaTime;
            // if (seconds >= 1) {
            //     Randomize();
            //     SetCustomizationValues();
            //     seconds = 0;
            // }
            if (ai != null)
            {
                cleanup();
                // do ai stuff
                

            }
        }

        private void FixedUpdate() {
            if (ai != null)
            {
                MakeEquip();
                CheckGearModifiers();
                CheckTerrainModifiers();
                Float(); 
                ai.move(this);
            }
        }

        private void cleanup()
        {
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
                Destroy(gameObject);
                return;
            }
            Ray ray = new Ray(transform.position, Vector3.down);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit))
            {
                Destroy(gameObject);
            }
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
        public void CheckTerrainModifiers()
        {
            falling = false;
            suffocationg = false;
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
                    if (transform.position.y < .25f)
                    {
                        ModifiedSpeed *= .5f;
                    }
                    if (!floating)
                    {
                        falling = true;
                        transform.position = new Vector3(transform.position.x, transform.position.y - (Time.deltaTime * .5f), transform.position.z);
                        if (transform.position.y <= -.5f)
                        {
                            new Vector3(transform.position.x, -.5f, transform.position.z);
                            Health -= Time.deltaTime * suffocationModifier;
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
                        ModifiedSpeed *= Mathf.Clamp((transform.position.y + .5f) * .65f, .01f, 1);
                    }
                    if (!floating)
                    {
                        falling = true;
                        transform.position = new Vector3(transform.position.x, transform.position.y - (Time.deltaTime * .1f), transform.position.z);
                        if (transform.position.y <= -.5f)
                        {
                            transform.position = new Vector3(transform.position.x, -.5f, transform.position.z);
                            suffocationg = true;
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
