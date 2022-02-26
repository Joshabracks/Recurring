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

        public override void Attack()
        {
            if (attackState == AttackState.Idle && set)
            {
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
                            }
                        }
                        dist = Vector3.Distance(center, c.transform.position);
                        if (dist <= 2) {
                            if (c.gear.innertube != null) 
                            {
                                c.gear.innertube.TakeDamage(damage * (2 - dist));
                            }
                            else
                            {
                                float damageDealt = damage * (2 - dist);
                                c.Health -= damageDealt;
                            }
                        }
                    }
                    attackState = AttackState.Reset;
                }
                else 
                {
                    Vector3 center = gameObject.GetComponentInChildren<Hammerhead>().transform.position;
                        float dist;
                        if (equippedCharacter.ai.mainCharacter.gear.umbrella != null) {
                            dist = Vector3.Distance(center, equippedCharacter.ai.mainCharacter.gear.umbrella.GetComponentInChildren<UmbrellaHead>().transform.position);
                            if (dist <= 2) {
                                equippedCharacter.ai.mainCharacter.gear.umbrella.TakeDamage(damage * (2 - dist));
                            }
                        }
                        dist = Vector3.Distance(center, equippedCharacter.ai.mainCharacter.transform.position);
                        if (dist <= 2) {
                            if (equippedCharacter.ai.mainCharacter.gear.innertube != null) 
                            {
                                equippedCharacter.ai.mainCharacter.gear.innertube.TakeDamage(damage * (2 - dist));
                            }
                            else
                            {
                                float damageDealt = damage * (2 - dist);
                                Debug.Log("HIT CREATURE FOR " + damageDealt.ToString());
                                equippedCharacter.ai.mainCharacter.Health -= damageDealt;
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
