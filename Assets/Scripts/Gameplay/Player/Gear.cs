using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player {

    public enum GearTypes {
        Innertube,
        Beaver,
        Balloon
    }
    public abstract class Gear : MonoBehaviour
    {
        private void Start() {
            gameObject.tag = "Gear";
        }
        public PlayerCharacter playerCharacter;
        public abstract void Equip();
        public abstract void Unequip();
        public abstract void PickUp();
        public abstract void Drop();
        public abstract void TakeDamage();
        public abstract void MoveModifier();
        public abstract void makeEquip();
        private void Update() {
            makeEquip();
        }
    }
}
