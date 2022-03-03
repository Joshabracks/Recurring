using UnityEngine;

namespace Gameplay.Player {

    public enum GearTypes {
        Innertube,
        Beaver,
        Balloon
    }
    public abstract class Gear : MonoBehaviour
    {
        public float health = 5;
        private void Start() {
            gameObject.tag = "Gear";
        }
        public Character equippedCharacter;
        public abstract void Equip(Character character);
        public abstract void Unequip();
        public abstract void PickUp(Character character);
        public abstract void Drop();
        public abstract void TakeDamage(float score);
        public abstract void MoveModifier();
        public abstract void makeEquip();
        abstract public void Randomize();
        abstract public void SetCustomizationValues();
    }
}
