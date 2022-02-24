using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player {

    public class Balloon : Gear
    {
        public override void makeEquip() {
            if (equippedCharacter != null) {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(equippedCharacter.transform.position.x - .5f, equippedCharacter.transform.position.y + 1, equippedCharacter.transform.position.z), Time.deltaTime * 10);

                Vector3 _direction = (equippedCharacter.transform.position - transform.position).normalized;
                if (_direction != Vector3.zero) {
                    //create the rotation we need to be in to look at the target        
                    Quaternion _lookRotation = Quaternion.LookRotation(_direction + Vector3.up);
    
                    //rotate us over time according to speed until we are in the required rotation
                    transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 3);
                }
            }
        }

        public override void Equip()
        {
            // playerCharacter.AllowedTerrain.Add(Terrain.TerrainType.Hole);
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
