using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player {

    public class Balloon : Gear
    {
        public override void makeEquip() {
            if (playerCharacter != null) {
                // if (transform.position.x != 0 || transform.position.y != 0 || transform.position.z != 0) {
                    // transform.position = Vector3.MoveTowards(transform.position, (new Vector3playerCharacter.transform.position) + 1, Time.deltaTime * 10);
                // }
                if (Vector3.Distance(transform.position, playerCharacter.transform.position) > 3) {
                    transform.position = Vector3.MoveTowards(transform.position, playerCharacter.transform.position, Vector3.Distance(playerCharacter.transform.position,transform.position) * Time.deltaTime * 2);
                } else {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerCharacter.transform.position.x, transform.position.y + 1, playerCharacter.transform.position.z), Vector3.Distance(playerCharacter.transform.position,transform.position) * Time.deltaTime);
                    // transform.position = new Vector3(transform.position.x, transform.position.y + (Time.deltaTime * 2), transform.position.z);
                }

                Vector3 _direction = (playerCharacter.transform.position - transform.position).normalized;
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
            // transform.parent = playerCharacter.transform;
            playerCharacter.AllowedTerrain.Add(Terrain.TerrainType.Hole);
        }

        public override void Unequip()
        {
            playerCharacter.AllowedTerrain.Remove(Terrain.TerrainType.Hole);
            playerCharacter = null;
            // transform.parent = null;
        }

        public override void PickUp()
        {
            playerCharacter.gear.Add(this);
            Equip();
        }
        public override void Drop()
        {
            playerCharacter.gear.Remove(this);
            Unequip();
        }

        public override void TakeDamage()
        {
            // do nothing
        }

        public override void MoveModifier()
        {
            // playerCharacter.ModifiedSpeed *= .8f;
            // if (playerCharacter.terrainType == Terrain.TerrainType.Water) 
            // {
                playerCharacter.ModifiedSpeed *= .8f;
                playerCharacter.floating = true;
                playerCharacter.targetFloatHeight += 1;
            // }
        }
    }
}
