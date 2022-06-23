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
                    Vector3 directionOfTravel = -equippedCharacter.transform.right;
                    Vector3 finalDirection = directionOfTravel + directionOfTravel.normalized * -.1f;
                    Vector3 targetPosition = equippedCharacter.transform.position + finalDirection;
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
            damage = UnityEngine.Random.Range(.1f, 1) * powerScale;
            speed = UnityEngine.Random.Range(.1f, 1) + powerScale;
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
        }
        public override void Attack()
        {
            if (cooldown > 0)
            {
                cooldown -= Time.deltaTime;
            }
            else
            {
                Bullet bullet = Instantiate(_bulletTemplate, Barrel.transform.position, transform.rotation);
                bullet.transform.rotation = equippedCharacter.transform.rotation;
                bullet.GetComponent<MeshRenderer>().material.SetColor("Color", cogColor);
                bullet.transform.localScale = new Vector3(.25f, .25f, .25f);
                bullet.life = range * UnityEngine.Random.Range(1f, 1.25f);
                bullet.damage = damage;
                bullet.character = equippedCharacter;
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
                            character.gunDamageLevel++;
                            character.LevelUpGunDamage();
                        break;
                    case GunBonus.Speed:
                        character.gunSpeedProgress ++;
                            character.gunSpeedLevel++;
                            character.LevelUpGunSpeed();
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

        }
    }
}
