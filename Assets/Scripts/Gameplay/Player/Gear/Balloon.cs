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
                // transform.position = Vector3.MoveTowards(transform.position, new Vector3(equippedCharacter.transform.position.x - .5f, equippedCharacter.transform.position.y + 1, equippedCharacter.transform.position.z), Time.deltaTime * 10);

                // Vector3 _direction = (equippedCharacter.transform.position - transform.position).normalized;
                // if (_direction != Vector3.zero) {
                //     //create the rotation we need to be in to look at the target        
                //     Quaternion _lookRotation = Quaternion.LookRotation(_direction + Vector3.up);

                //     //rotate us over time according to speed until we are in the required rotation
                //     transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 3);
                // }
            }
        }

        public override void Randomize()
        {
            color = Random.ColorHSV(0, 1);
            health = Random.Range(1, 10);
        }

        public override void SetCustomizationValues()
        {
            gameObject.GetComponent<MeshRenderer>().material.SetColor("Color", color);
        }

        public override void Equip()
        {
            transform.parent = equippedCharacter.transform;
            transform.rotation = equippedCharacter.transform.rotation;
        }
        public override void Unequip()
        {
            // playerCharacter.AllowedTerrain.Remove(Terrain.TerrainType.Hole);
            // List<Gear> gear = equippedCharacter.gear;
            // equippedCharacter.gear = new List<Gear>();
            // foreach (Gear g in gear) {
            //     if (g != this) {
            //         equippedCharacter.gear.Add(g);
            //     }
            // }
            // equippedCharacter.gear.Remove(this as Gear);
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
            // playerCharacter.gear.Remove(this);
            Unequip();
        }

        public override void TakeDamage()
        {
            // do nothing
        }

        public override void MoveModifier()
        {
            equippedCharacter.ModifiedSpeed *= .8f;
            equippedCharacter.floating = true;
            equippedCharacter.targetFloatHeight += 1;
        }
    }
}
