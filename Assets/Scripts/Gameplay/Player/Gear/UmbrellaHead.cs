using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player
{

    public class UmbrellaHead : MonoBehaviour
    {
        public Umbrella umbrella;
        public void TakeDamage(float damage) {
            umbrella.TakeDamage(damage);
        }
    }
}
