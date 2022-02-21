using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player
{
    
    public class PlayerCharacter
    {
        public List<Gear> gear;
        public int Health = 10;
        public int MaxHealth = 10;
        public float Speed = .2f;
        public Vector2 movement;
        public PlayerCharacter() {
            gear = new List<Gear>();
            movement = new Vector2();
        }
    }
}
