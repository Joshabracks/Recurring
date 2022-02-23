using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player {

    public class Innertube : Gear
    {
        public override void makeEquip() {
            if (playerCharacter != null) {
                if (transform.position.x != 0 || transform.position.y != 0 || transform.position.z != 0) {
                    transform.position = Vector3.MoveTowards(transform.position, playerCharacter.transform.position, Time.deltaTime * 10);
                }
            }
        }

        public override void Equip()
        {
            transform.parent = playerCharacter.transform;
            playerCharacter.AllowedTerrain.Add(Terrain.TerrainType.Water);
        }

        public override void Unequip()
        {
            playerCharacter = null;
            transform.parent = null;
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
            playerCharacter.ModifiedSpeed *= .8f;
            if (playerCharacter.terrainType == Terrain.TerrainType.Water) 
            {
                playerCharacter.ModifiedSpeed *= .6f;
                playerCharacter.floating = true;
                playerCharacter.targetFloatHeight = 0;
            }
        }
    }
}
