using UnityEngine;

namespace Gameplay.Player
{

    public class Umbrella : Gear
    {
        private bool set = false;
        private float angle = 0;
        private bool hasBalloon = false;
        public GameObject top;
        Color color1;
        Color color2;

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
                    Vector3 directionOfTravel = equippedCharacter.transform.forward;
                    Vector3 finalDirection = directionOfTravel + directionOfTravel.normalized * -.5f;
                    Vector3 targetPosition = equippedCharacter.transform.position + finalDirection;
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 10);

                    if (transform.position == targetPosition)
                    {
                        set = true;
                    }
                }
                else
                {
                    hasBalloon = equippedCharacter && equippedCharacter.gear.balloon != null;
                    if (equippedCharacter.floating && !hasBalloon)
                    {
                        if (angle > 0)
                        {
                            angle -= Time.deltaTime * 100;
                            transform.rotation = Quaternion.AngleAxis(angle, equippedCharacter.transform.right);
                        }
                    }
                    else
                    {
                        if (angle < 60)
                        {
                            angle += Time.deltaTime * 100;
                            transform.rotation = Quaternion.AngleAxis(angle, equippedCharacter.transform.right);
                        }
                    }
                }

            }
        }

        public override void Randomize()
        {
            color1 = Random.ColorHSV(0, 1);
            color2 = Random.ColorHSV(0, 1);
            health = Random.Range(1, 10);
        }

        public override void SetCustomizationValues()
        {
            top.GetComponent<MeshRenderer>().material.SetColor("Color_08f30f87981049c7a12b5478743a0dce", color1);
            top.GetComponent<MeshRenderer>().material.SetColor("Color_e81d83b4d7f34ab39bc7f7a9e9aa3cda", color2);
        }

        public override void Equip(Character character)
        {
            equippedCharacter = character;
            if (equippedCharacter.gear.umbrella != null)
            {
                equippedCharacter.gear.umbrella.Drop();
            }
            equippedCharacter.gear.umbrella = this;
            transform.parent = equippedCharacter.transform;
            transform.rotation = equippedCharacter.transform.rotation;
        }

        public override void Unequip()
        {
            equippedCharacter.gear.umbrella = null;
            equippedCharacter = null;
            transform.parent = null;
        }

        public override void PickUp(Character character)
        {
            if (character.gear.umbrella == null || character.gear.umbrella.health < health)
            {
                Equip(character);
            }
        }
        public override void Drop()
        {
            set = false;
            Unequip();
        }

        public override void TakeDamage(float score)
        {
            if (equippedCharacter != null)
            {
                if (equippedCharacter.ai != null)
                {
                    equippedCharacter.Health -= score / 3;
                }
                else 
                {
                    health -= score;
                }
            }

            if (health < 0)
            {
                Drop();
                Destroy(gameObject);
            }
            // do nothing
        }

        public override void MoveModifier()
        {
            if (equippedCharacter.terrainType == Terrain.TerrainType.Lava)
            {
                equippedCharacter.floating = true;
                equippedCharacter.targetFloatHeight += 1;
            }
            else if (equippedCharacter.transform.position.y > .5f || (equippedCharacter.terrainType == Terrain.TerrainType.Hole && !equippedCharacter.floating && equippedCharacter.transform.position.y > -2))
            {
                if (!hasBalloon)
                {
                    equippedCharacter.floating = true;
                    equippedCharacter.targetFloatHeight = -2;
                }
            }
        }
    }
}
