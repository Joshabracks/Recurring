using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player
{

    public class Hammer : Weapon
    {
        private bool set = false;
        private float angle = 0;
        public GameObject head;
        public Color color;

        private enum AttackState
        {
            Idle,
            Strike,
            Reset
        }

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
                    angle += Time.deltaTime * 200;
                    transform.rotation = Quaternion.AngleAxis(angle, equippedCharacter.transform.right);
                }
                else
                {
                    // do the thing
                    attackState = AttackState.Reset;
                }
            }
        }


        public override void Equip()
        {
            transform.parent = equippedCharacter.transform;
            transform.rotation = equippedCharacter.transform.rotation;
        }

        public override void Unequip()
        {
            equippedCharacter.Unequip(this);
            equippedCharacter = null;
        }

        public override void PickUp()
        {
            equippedCharacter.gear.Add(this);
            Equip();
        }
        public override void Drop()
        {
            Unequip();
        }

        public override void TakeDamage()
        {
            // do nothing
        }

        public override void MoveModifier()
        {
            // equippedCharacter.ModifiedSpeed *= .85f
        }
    }
}
