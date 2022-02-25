using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player {


    public abstract class Weapon : Gear
    {
        public float damage;
        public float powerScale = .25f;
        abstract public void Attack();
    }
}
