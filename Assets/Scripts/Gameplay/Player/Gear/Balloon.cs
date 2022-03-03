using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player
{
    public class Balloon : Gear
    {
        public Color color;
        public bool set = false;
        public override void makeEquip()
        {
            if (equippedCharacter != null)
            {
                if (!set)
                {

                    Vector3 directionOfTravel = -equippedCharacter.transform.forward;
                    Vector3 finalDirection = directionOfTravel + directionOfTravel.normalized * .1f;
                    Vector3 targetPosition = equippedCharacter.transform.position + finalDirection;
                    targetPosition = new Vector3(targetPosition.x, targetPosition.y + 4, targetPosition.z);
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 10);

                    if (transform.position == targetPosition)
                    {
                        set = true;
                    }
                }
            }
        }

        public override void Randomize()
        {
            color = Random.ColorHSV(0, 1);
            health = Random.Range(1, 10);
        }

        public override void SetCustomizationValues()
        {
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", color);
        }

        public override void Equip(Character character)
        {
            if (character.gear.balloon != null) {
                character.gear.balloon.Drop();
            }
            equippedCharacter = character;
            equippedCharacter.gear.balloon = this;
            transform.parent = equippedCharacter.transform;
            transform.rotation = equippedCharacter.transform.rotation;
        }
        public override void Unequip()
        {
            transform.parent = null;
            equippedCharacter.gear.balloon = null;
            equippedCharacter = null;
        }

        public override void PickUp(Character character)
        {
            if (character.gear.balloon == null) {
                Equip(character);
            }
            else if (character.gear.balloon.health < health)
            {
                Equip(character);
            }
        }
        public override void Drop()
        {
            set = false;
            Unequip();
        }

        public override void TakeDamage(float score)
        {
            health -= score;
        }

        public override void MoveModifier()
        {
            equippedCharacter.ModifiedSpeed *= .8f;
            equippedCharacter.floating = true;
            equippedCharacter.targetFloatHeight += 1;
        }
    }
}
