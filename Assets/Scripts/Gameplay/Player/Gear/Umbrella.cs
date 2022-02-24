using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player {

    public class Umbrella : Gear
    {
        private bool set = false;
        public override void makeEquip() {
            if (equippedCharacter != null) {
                // transform.position = Vector3.MoveTowards(transform.position, new Vector3(equippedCharacter.transform.position.x + .5f, equippedCharacter.transform.position.y, equippedCharacter.transform.position.z), Time.deltaTime * 10);
                if (!set) {
                    if (Vector2.Distance(new Vector2(equippedCharacter.transform.position.x, equippedCharacter.transform.position.z), new Vector2(transform.position.x, transform.position.z)) > 0.5f) {
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(equippedCharacter.transform.position.x + .5f, equippedCharacter.transform.position.y, equippedCharacter.transform.position.z), Time.deltaTime * 10);
                    }
                    else 
                    {
                        set = true;
                    }
                }
                // if (equippedCharacter.terrainType == Terrain.TerrainType.Lava) {
                //     transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, new Vector3(56.3f, 0, 0), Time.deltaTime * 3, Time.deltaTime * 3));
                // } else {
                //     transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, new Vector3(0, 0, 0), Time.deltaTime * 3, Time.deltaTime * 3));
                // }
            }
        }

        public override void Equip()
        {
                        transform.parent = equippedCharacter.transform;

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
            if (equippedCharacter.terrainType == Terrain.TerrainType.Lava) {
                equippedCharacter.floating = true;
                equippedCharacter.targetFloatHeight += 1;
            }
        }
    }
}
