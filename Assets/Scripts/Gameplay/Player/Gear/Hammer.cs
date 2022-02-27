using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.State;

namespace Gameplay.Player
{

    public class Hammer : Weapon
    {
        private bool set = false;
        private float angle = 0;
        public GameObject head;
        public Color color;
        public float unequippable = 0;
        public AudioClip[] swingWeapon;
        // public AudioClip[] hitGround;
        public AudioClip[] hitFlesh;
        public AudioClip[] hitArmor;
    
        // public List<GameObject> attackMap;
        private enum AttackState
        {
            Idle,
            Strike,
            Reset
        }

        // private void Awake() {
        //     attackMap = new List<GameObject>();
        // }

        private AttackState attackState = AttackState.Idle;

        public override void makeEquip()
        {
            if (equippedCharacter != null)
            {
                if (equippedCharacter.Health <= 0)
                {
                    return;
                }
                if (!set)
                {
                    Vector3 directionOfTravel = equippedCharacter.transform.right;
                    Vector3 finalDirection = directionOfTravel + directionOfTravel.normalized * .1f;
                    Vector3 targetPosition = equippedCharacter.transform.position + finalDirection;
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 10);

                    if (transform.position == targetPosition)
                    {
                        set = true;
                    }
                }
                if (attackState == AttackState.Strike) {
                    Attack();
                }
                if (attackState == AttackState.Reset) {
                    if (angle > 0)
                {
                    angle -= Time.deltaTime * 100;
                    transform.rotation = Quaternion.AngleAxis(angle, equippedCharacter.transform.right);
                }
                else
                {
                    // do the thing
                    attackState = AttackState.Idle;
                }
                }
            }
        }

        public override void Randomize()
        {
            damage = Random.Range(2f, 4f) * powerScale;
            color = Random.ColorHSV(0, 1);
        }

        public override void SetCustomizationValues()
        {
            head.GetComponent<MeshRenderer>().material.SetColor("Color", color);
        }

        public void playSound(AudioClip[] clip, float pitchMin, float pitchMax) {
            AudioSource source = GetComponent<AudioSource>();
            source.clip = clip[Random.Range(0, clip.Length - 1)];
            source.pitch = Random.Range(pitchMin, pitchMax);
            source.Play();
        }

        public override void Attack()
        {
            if (attackState == AttackState.Idle && set)
            {
                playSound(swingWeapon, .8f, 1);
                attackState = AttackState.Strike;
            }

            if (attackState == AttackState.Strike)
            {
                if (angle < 90)
                {
                    angle += Time.deltaTime * 600;
                    transform.rotation = Quaternion.AngleAxis(angle, transform.right);
                }
                else if (equippedCharacter.ai == null)
                {
                    // do the thing
                    // Debug.Log(attackMap.Count);
                    foreach (Character c in GameObject.Find("StateMachine").GetComponent<StateMachine>()._characterContainer.GetComponentsInChildren<Character>())
                    {
                        if (c == this) 
                        {
                            continue;
                        }
                        Vector3 center = gameObject.GetComponentInChildren<Hammerhead>().transform.position;
                        float dist;
                        if (c.gear.umbrella != null) {
                            dist = Vector3.Distance(center, c.gear.umbrella.GetComponentInChildren<UmbrellaHead>().transform.position);
                            if (dist <= 2) {
                                c.gear.umbrella.TakeDamage(damage * (2 - dist));
                                playSound(hitArmor, .9f, 1.1f);
                            }
                        }
                        dist = Vector3.Distance(center, c.transform.position);
                        if (dist <= 2) {
                            if (c.gear.innertube != null) 
                            {
                                c.gear.innertube.TakeDamage(damage * (2 - dist));
                                playSound(hitArmor, .9f, 1.1f);
                            }
                            else
                            {
                                float damageDealt = damage * (2 - dist);
                                c.TakeDamage(damageDealt);
                                playSound(hitFlesh, .9f, 1.1f);
                            }
                        }
                    }
                    // playSound(hitGround, .8f, 1);
                    attackState = AttackState.Reset;
                }
                else 
                {
                    Vector2 center = new Vector2(gameObject.GetComponentInChildren<Hammerhead>().transform.position.x, gameObject.GetComponentInChildren<Hammerhead>().transform.position.z);
                        float dist;
                        if (equippedCharacter.ai.mainCharacter.gear.umbrella != null) {
                            Vector2 uPos = equippedCharacter.ai.mainCharacter.gear.umbrella.GetComponentInChildren<UmbrellaHead>().transform.position;
                            dist = Vector2.Distance(center, uPos);
                            if (dist <= 2) {
                                playSound(hitArmor, .9f, 1.1f);
                                equippedCharacter.ai.mainCharacter.gear.umbrella.TakeDamage(damage * (2 - dist));
                            }
                        }
                        dist = Vector3.Distance(center, new Vector2(equippedCharacter.ai.mainCharacter.transform.position.x, equippedCharacter.ai.mainCharacter.transform.position.z));
                        if (dist <= 2) {
                            if (equippedCharacter.ai.mainCharacter.gear.innertube != null) 
                            {
                                equippedCharacter.ai.mainCharacter.gear.innertube.TakeDamage(damage * (2 - dist));
                                playSound(hitArmor, .9f, 1.1f);
                            }
                            else
                            {
                                float damageDealt = damage * (2 - dist);
                                equippedCharacter.ai.mainCharacter.TakeDamage(damageDealt);
                                playSound(hitFlesh, .9f, 1.1f);
                            }
                        }
                    attackState = AttackState.Reset;
                }
            }
        }


        public override void Equip(Character character)
        {
            if (character.gear.hammer != null) {
                character.gear.hammer.Drop();
            }
            equippedCharacter = character;
            equippedCharacter.gear.hammer = this;
            transform.parent = equippedCharacter.transform;
            transform.rotation = equippedCharacter.transform.rotation;
        }

        public override void Unequip()
        {
            transform.parent = null;
            equippedCharacter.gear.hammer = null;
            equippedCharacter = null;
        }

        public override void PickUp(Character character)
        {
            if (unequippable > 0) {
                unequippable -= Time.deltaTime;
                return;
            }
            
            Equip(character);
        }
        public override void Drop()
        {
            set = false;
            Unequip();
        }

        public override void TakeDamage(float score)
        {
            // do nothing
        }

        public override void MoveModifier()
        {
            // equippedCharacter.ModifiedSpeed *= .85f
        }
    }
}
