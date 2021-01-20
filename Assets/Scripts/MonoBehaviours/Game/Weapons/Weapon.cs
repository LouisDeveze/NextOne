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

        public AnimatorOverrideController weaponAnimator;
        

        public virtual void Create(Animator animator, Transform rightHand, Transform leftHand) { }

        /// <summary>  Callback launched when the player is going forward the direction he's looking  </summary>
        /// <param name="animator"> Animator of the player </param>
        /// <param name="model"> Model used to change transform </param>
        public virtual void OnMoveForward(Animator animator, GameObject model) { }

        /// <summary>  Callback launched when the player is going backward the direction he's looking  </summary>
        /// <param name="animator"> Animator of the player </param>
        /// <param name="model"> Model used to change transform </param>
        public virtual void OnMoveBackward(Animator animator, GameObject model) { }

        /// <summary>  Callback launched when the player is going Lefward the direction he's looking  </summary>
        /// <param name="animator"> Animator of the player </param>
        /// <param name="model"> Model used to change transform </param>
        public virtual void OnMoveLeftward(Animator animator, GameObject model) { }

        /// <summary>  Callback launched when the player is going rightward the direction he's looking  </summary>
        /// <param name="animator"> Animator of the player </param>
        /// <param name="model"> Model used to change transform </param>
        public virtual void OnMoveRightward(Animator animator, GameObject model) { }
        

        public virtual void OnMainSpell() {}

        public virtual void OnSecondarySpell() { }

        public virtual void OnAutoAttack() { }


    }
}
