using System.Collections.Generic;
using UnityEngine;
using Gameplay.Terrain;

namespace Gameplay.Player
{
    
    public class PlayerCharacter : MonoBehaviour
    {
        public List<Gear> gear;
        public int Health = 10;
        public int MaxHealth = 10;
        public float Speed = .2f;
        public float ModifiedSpeed = .2f;
        public Vector2 movement;
        public TerrainType terrainType;
        public List<TerrainType> AllowedTerrain;
        public bool floating = false;
        public float targetFloatHeight = 0;
        public float verticalVelocity = 0;
        public PlayerCharacter() {
            gear = new List<Gear>();
            movement = new Vector2();
            AllowedTerrain = new List<TerrainType>();
            AllowedTerrain.Add(TerrainType.Dirt);
            AllowedTerrain.Add(TerrainType.Grass);
            AllowedTerrain.Add(TerrainType.Sand);
        }

        public void CheckGearModifiers() {
            floating = false;
            ModifiedSpeed = Speed;
            foreach (Gear g in gear) {
                g.MoveModifier();
            }
        }
    }
}
