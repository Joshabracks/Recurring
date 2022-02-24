using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player {

    public class Umbrella : Gear
    {
        private bool set = false;
        private float angle = 0;

        public override void makeEquip() {
            if (equippedCharacter != null) {
                if (equippedCharacter.Health <= 0) {
                    return;
                }
                if (!set) {
                    if (Vector2.Distance(new Vector2(equippedCharacter.transform.position.x, equippedCharacter.transform.position.z), new Vector2(transform.position.x, transform.position.z)) > 0.5f) {
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(equippedCharacter.transform.position.x + .5f, equippedCharacter.transform.position.y, equippedCharacter.transform.position.z), Time.deltaTime * 10);
                    }
                    else 
                    {
                        set = true;
                    }
                } else {

                    if (equippedCharacter.floating) {
                        if (angle > 0) {
                            angle -= Time.deltaTime * 100;
                            transform.rotation = Quaternion.AngleAxis(angle, equippedCharacter.transform.right);
                        }
                    } else {
                        if (angle < 60) {
                            angle += Time.deltaTime * 100;
                            transform.rotation = Quaternion.AngleAxis(angle, equippedCharacter.transform.right);
                        }
                    }                
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
            if (equippedCharacter.terrainType == Terrain.TerrainType.Lava) {
                equippedCharacter.floating = true;
                equippedCharacter.targetFloatHeight += 1;
            } else if (equippedCharacter.transform.position.y > .5f || (equippedCharacter.terrainType == Terrain.TerrainType.Hole && !equippedCharacter.floating && equippedCharacter.transform.position.y > -2)) {
                equippedCharacter.floating = true;
                equippedCharacter.targetFloatHeight = -2;
            }
        }
    }
}
