using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player {


    public abstract class Weapon : Gear
    {
        abstract public void Attack();
    }
}
