using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Gameplay.Player
{

    public class Gun : Weapon
    {
        public enum GunBonus
        {
            Damage,
            Speed
        }

        public GunBonus gunBonus;
        private bool set = false;
        public float range = 5;
        public float speed = 1;
        // public float spread = 0;
        public Color barrelColor;
        public Color bodyColor;
        public Color cogColor;
        public GameObject Barrel;
        public GameObject Body;
        public GameObject Cog;
        public Bullet _bulletTemplate;
        private float unequippable = 0;

        private float cooldown = 0;

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
                    // Vector3 targetPosition = new Vector3(
                    //     equippedCharacter.transform.position.x - .9f, 
                    //     equippedCharacter.transform.position.y, 
                    //     equippedCharacter.transform.position.z - .5f
                    //     ) + equippedCharacter.transform.TransformDirection(Vector3.forward);
                    Vector3 directionOfTravel = -equippedCharacter.transform.right;
                    Vector3 finalDirection = directionOfTravel + directionOfTravel.normalized * -.1f;
                    Vector3 targetPosition = equippedCharacter.transform.position + finalDirection;
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 10);

                    if (transform.position == targetPosition)
                    {
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

        public override void Randomize()
        {
            damage = UnityEngine.Random.Range(.1f, 1) * powerScale;
            speed = UnityEngine.Random.Range(.1f, 1) + powerScale;
            // spread = Random.Range(.1f, 1);
            int gunBonuses = Enum.GetNames(typeof(GunBonus)).Length;
            gunBonus = (GunBonus)UnityEngine.Random.Range(0, gunBonuses);
            barrelColor = UnityEngine.Random.ColorHSV(0f, (int)gunBonus / gunBonuses);
            bodyColor = UnityEngine.Random.ColorHSV(0f, (int)gunBonus / gunBonuses);
            cogColor = UnityEngine.Random.ColorHSV(0f, (int)gunBonus / gunBonuses);

        }

        public override void SetCustomizationValues()
        {
            Body.gameObject.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", bodyColor);
            Cog.gameObject.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", cogColor);
            Barrel.gameObject.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", barrelColor);
            // Barrel.transform.localScale = new Vector3(.65f + range, 1, 1);
            // Cog.transform.localScale = new Vector3(1 + spread, 1, 1);
            // Body.transform.localScale = new Vector3(1 + damage, 1, 1);

        }
        public override void Attack()
        {
            // Barrel.transform.rotation = new Quaternion(Barrel.transform.rotation.x + (Time.deltaTime * 10), Barrel.transform.rotation.y, Barrel.transform.rotation.z, Barrel.transform.rotation.w);
            // Cog.transform.rotation = new Quaternion(Cog.transform.rotation.x - (Time.deltaTime * 10), Cog.transform.rotation.y, Cog.transform.rotation.z, Cog.transform.rotation.w);

            if (cooldown > 0)
            {
                cooldown -= Time.deltaTime;
            }
            else
            {
                Bullet bullet = Instantiate(_bulletTemplate, Barrel.transform.position, transform.rotation);
                bullet.transform.rotation = equippedCharacter.transform.rotation;
                // bullet.transform.rotation = Quaternion.AngleAxis(Random.Range(-spread * 10, spread * 10), bullet.transform.up);
                bullet.GetComponent<MeshRenderer>().material.SetColor("Color", cogColor);
                bullet.transform.localScale = new Vector3(.25f, .25f, .25f);
                bullet.life = range * UnityEngine.Random.Range(1f, 1.25f);
                bullet.damage = damage;
                cooldown = 1 / speed;
            }
        }

        public override void Equip(Character character)
        {
            if (character.gear.gun != null)
            {
                character.gear.gun.Drop();
            }
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
            if (character.ai == null && character.gear.gun != null)
            {

                switch (gunBonus)
                {
                    case GunBonus.Damage:
                        // character.gunDamageProgress ++;
                        // if (character.gunDamageProgress > character.gunDamageLevel) {
                            // character.gunDamageProgress = 0;
                            character.gunDamageLevel++;
                            character.LevelUpGunDamage();
                        // }
                        break;
                    case GunBonus.Speed:
                        character.gunSpeedProgress ++;
                        // if (character.gunSpeedProgress > character.gunSpeedLevel) {
                            // // character.gunSpeedProgress = 0;
                            character.gunSpeedLevel++;
                            character.LevelUpGunSpeed();
                        // }
                        break;
                }
                Destroy(gameObject);
                return;

            }
            if (unequippable > 0)
            {
                unequippable -= Time.deltaTime;
                return;
            }

            Equip(character);
        }
        public override void Drop()
        {
            set = false;
            Unequip();
            unequippable = 5;
        }

        public override void TakeDamage(float score)
        {
            health -= score;
            if (health < 0)
            {
                Drop();
                Destroy(gameObject);
            }
        }

        public override void MoveModifier()
        {
            // equippedCharacter.ModifiedSpeed *= .9f;
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
