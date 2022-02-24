using System.Collections.Generic;
using UnityEngine;
using Gameplay.Terrain;

namespace Gameplay.Player
{
    
    public class Character : MonoBehaviour
    {
        public List<Gear> gear;
        public float Health = 10;
        public float MaxHealth = 10;
        public float Speed = .2f;
        public float ModifiedSpeed = .2f;
        public Vector2 movement;
        public TerrainType terrainType;
        // public List<TerrainType> AllowedTerrain;
        public bool floating = false;
        public bool falling = false;
        public bool suffocationg = false;
        public bool burning = false;
        public float targetFloatHeight = 0;
        public float verticalVelocity = 0;
        private float suffocationModifier = .4f;
        public Character() {
            gear = new List<Gear>();
            movement = new Vector2();
            // AllowedTerrain = new List<TerrainType>();
            // AllowedTerrain.Add(TerrainType.Dirt);
            // AllowedTerrain.Add(TerrainType.Grass);
            // AllowedTerrain.Add(TerrainType.Sand);
        }

        public void CheckGearModifiers() {
            floating = false;
            ModifiedSpeed = Speed;
            targetFloatHeight = 0.5f;
            foreach (Gear g in gear) {
                g.MoveModifier();
            }
        }

        public void Unequip(Gear g) {
            gear.Remove(g);
        }

        public void MakeEquip() {
            foreach (Gear g in gear) {
                g.makeEquip();
            }

            int i = 0;
            while (i < gear.Count) {
                Gear g = gear[i];
                if (g.health < 0) {
                    g.Unequip();
                    Unequip(g);
                    Destroy(g);
                    Destroy(g.gameObject);
                    i = 0;
                } else {
                    i++;
                }
            }
        }
        public void CheckTerrainModifiers() {
            falling = false;
            suffocationg = false;
            burning = false;
            switch(terrainType) {
                case TerrainType.Grass:
                    break;
                case TerrainType.Dirt:
                    ModifiedSpeed *= .9f;
                    break;
                case TerrainType.Rock:
                    ModifiedSpeed *= 1.1f;
                    break;
                case TerrainType.Water:
                    if (transform.position.y < .25f) {
                        ModifiedSpeed *= .5f;
                    }
                    if (!floating) {
                        falling = true;
                        transform.position = new Vector3(transform.position.x, transform.position.y - (Time.deltaTime * .2f), transform.position.z);
                        if (transform.position.y <= -.5f) {
                            new Vector3(transform.position.x, -.5f, transform.position.z);
                            Health -= Time.deltaTime * suffocationModifier;
                        }
                    }
                    break;
                case TerrainType.Hole:
                    if (!floating) {
                        falling = true;
                        transform.position = new Vector3(transform.position.x, transform.position.y - (Time.deltaTime * 3), transform.position.z);
                        if (transform.position.y < -2) {
                            Health = 0;
                        }
                    }
                    break;
                case TerrainType.Sand:
                    if (!floating) {
                        ModifiedSpeed *= .65f;
                    }
                    break;
                case TerrainType.QuickSand:
                    if (transform.position.y < .5f) {
                        ModifiedSpeed *= Mathf.Clamp((transform.position.y + .5f) * .65f, .01f, 1);
                    }
                    if (!floating) {
                        falling = true;
                        transform.position = new Vector3(transform.position.x, transform.position.y - (Time.deltaTime * .1f), transform.position.z);
                        if (transform.position.y <= -.5f) {
                            transform.position = new Vector3(transform.position.x, -.5f, transform.position.z);
                            suffocationg = true;
                        }
                    }
                    break;
                case TerrainType.Lava:
                    foreach ( Gear g in gear) {
                        if (g.GetType() == typeof(Balloon)) {
                            Balloon b = g as Balloon;
                            b.health -= Time.deltaTime * 3;
                        }
                    }
                    if (!floating) {
                        burning = true;
                        ModifiedSpeed *= Health / MaxHealth;
                        Health -= Time.deltaTime * 3;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
