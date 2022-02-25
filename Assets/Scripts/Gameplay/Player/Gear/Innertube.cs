using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player {

    public class Innertube : Gear
    {
        Color color;
        public override void makeEquip() {
            if (equippedCharacter != null) {
                if (transform.position.x != 0 || transform.position.y != 0 || transform.position.z != 0) {
                    transform.position = Vector3.MoveTowards(transform.position, equippedCharacter.transform.position, Time.deltaTime * 10);
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
            gameObject.GetComponent<MeshRenderer>().material.SetColor("Color", color);
        }

        public override void Equip()
        {
            transform.parent = equippedCharacter.transform;
            // playerCharacter.AllowedTerrain.Add(Terrain.TerrainType.Water);
        }

        public override void Unequip()
        {
            equippedCharacter = null;
            transform.parent = null;
        }

        public override void PickUp()
        {
            equippedCharacter.gear.Add(this);
            Equip();
        }
        public override void Drop()
        {
            equippedCharacter.gear.Remove(this);
            Unequip();
        }

        public override void TakeDamage()
        {
            // do nothing
        }

        public override void MoveModifier()
        {
            equippedCharacter.ModifiedSpeed *= .8f;
            if (equippedCharacter.terrainType == Terrain.TerrainType.Water) 
            {
                if (equippedCharacter.targetFloatHeight < 0.5f) {
                    equippedCharacter.ModifiedSpeed *= .6f;
                }
                equippedCharacter.floating = true;
                equippedCharacter.targetFloatHeight -= 0.5f;
            }
        }
    }
}
