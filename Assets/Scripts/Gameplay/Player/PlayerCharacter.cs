using System.Collections.Generic;
using UnityEngine;
using Gameplay.Terrain;

namespace Gameplay.Player
{
    
    public class PlayerCharacter
    {
        public List<Gear> gear;
        public int Health = 10;
        public int MaxHealth = 10;
        public float Speed = .2f;
        public Vector2 movement;
        public List<TerrainType> AllowedTerrain;
        public PlayerCharacter() {
            gear = new List<Gear>();
            movement = new Vector2();
            AllowedTerrain = new List<TerrainType>();
            AllowedTerrain.Add(TerrainType.Dirt);
            AllowedTerrain.Add(TerrainType.Grass);
            AllowedTerrain.Add(TerrainType.Sand);
        }
    }
}
