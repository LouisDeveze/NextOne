using UnityEngine;

namespace NextOne
{
    
    public class Weapon : ScriptableObject
    {
        // Name of the Weapon
        public string Name;

        public ScriptableObject MainSpell;
        public ScriptableObject SecondarySpell;
        public ScriptableObject BasicAttack;

        public string idleAnimation;


        public virtual void Create(Transform rightHand, Transform leftHand) { }

        public virtual void OnMainSpell() {}

        public virtual void OnSecondarySpell() { }

        public virtual void OnAutoAttack() { }
    }
}
