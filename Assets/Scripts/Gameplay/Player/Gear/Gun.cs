using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player {

    public class Gun : Weapon
    {
        private bool set = false;
        public float range = 5;
        public float projectiles = 1;
        public float spread = 0;
        public Color barrelColor;
        public Color bodyColor;
        public Color cogColor;
        public GameObject Barrel;
        public GameObject Body;
        public GameObject Cog;
        private float unequippable = 0;

        public override void makeEquip() {
            if (equippedCharacter != null) {
                if (equippedCharacter.Health <= 0) {
                    return;
                } 
                if (!set) {
                    // Vector3 targetPosition = new Vector3(
                    //     equippedCharacter.transform.position.x - .9f, 
                    //     equippedCharacter.transform.position.y, 
                    //     equippedCharacter.transform.position.z - .5f
                    //     ) + equippedCharacter.transform.TransformDirection(Vector3.forward);
                    Vector3 directionOfTravel = -equippedCharacter.transform.right;
                    Vector3 finalDirection = directionOfTravel + directionOfTravel.normalized * -.1f;
                    Vector3 targetPosition = equippedCharacter.transform.position + finalDirection;
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 10);

                    if (transform.position == targetPosition) {
                        set = true;
                    }
                //     if (Vector2.Distance(new Vector2(equippedCharacter.transform.position.x, equippedCharacter.transform.position.z), new Vector2(transform.position.x, transform.position.z)) > 0.75f) {
                //         transform.position = Vector3.MoveTowards(transform.position, new Vector3(equippedCharacter.transform.position.x + .75f, equippedCharacter.transform.position.y, equippedCharacter.transform.position.z), Time.deltaTime * 10);
                // //             transform.rotation = Quaternion.AngleAxis(angle, equippedCharacter.transform.right);
                //     }
                //     else 
                //     {
                //         set = true;
                //     }
                // } else {

                //     if (equippedCharacter.floating) {
                //         if (angle > 0) {
                //             angle -= Time.deltaTime * 100;
                //         }
                //     } else {
                //         if (angle < 60) {
                //             angle += Time.deltaTime * 100;
                //             transform.rotation = Quaternion.AngleAxis(angle, equippedCharacter.transform.right);
                //         }
                //     }                
                }
        
            }
        }
        
        public override void Randomize() {
            damage = Random.Range(.1f, 1) * powerScale;
            projectiles = Random.Range(.1f, 1) * powerScale;
            spread = Random.Range(.1f, 1);
            barrelColor = Random.ColorHSV(0, 1);
            bodyColor = Random.ColorHSV(0, 1);
            cogColor = Random.ColorHSV(0, 1);

        }

        public override void SetCustomizationValues()
        {
            Body.GetComponent<MeshRenderer>().material.SetColor("Color", bodyColor);
            Cog.GetComponent<MeshRenderer>().material.SetColor("Color", cogColor);
            Barrel.GetComponent<MeshRenderer>().material.SetColor("Color", barrelColor);
            Barrel.transform.localScale = new Vector3(.65f + range, 1, 1);
            Cog.transform.localScale = new Vector3(1 + spread, 1, 1);
            Body.transform.localScale = new Vector3(1 + damage, 1, 1);

        }
        public override void Attack()
        {
        
        }

        public override void Equip(Character character)
        {
            equippedCharacter = character;
            equippedCharacter.gear.gun = this;
            transform.parent = character.transform;
            transform.rotation = character.transform.rotation;
        }

        public override void Unequip()
        {
            equippedCharacter.gear.gun = null;
            equippedCharacter = null;
            transform.parent = null;
        }

        public override void PickUp(Character character) 
        {
            if (unequippable > 0) {
                unequippable -= Time.deltaTime;
                return;
            }
            if (character.gear.gun != null) {
                character.gear.gun.Drop();
            }
            Equip(character);
        }
        public override void Drop()
        {
            Unequip();
            unequippable = 5;
        }

        public override void TakeDamage(float score)
        {
            health -= score;
            if (health < 0) {
                Drop();
                Destroy(gameObject);
            }
        }

        public override void MoveModifier()
        {
            equippedCharacter.ModifiedSpeed *= .9f;
            // if (equippedCharacter.terrainType == Terrain.TerrainType.Lava) {
            //     equippedCharacter.floating = true;
            //     equippedCharacter.targetFloatHeight += 1;
            // } else if (equippedCharacter.transform.position.y > .5f || (equippedCharacter.terrainType == Terrain.TerrainType.Hole && !equippedCharacter.floating && equippedCharacter.transform.position.y > -2)) {
            //     equippedCharacter.floating = true;
            //     equippedCharacter.targetFloatHeight = -2;
            // }
        }
    }
}
