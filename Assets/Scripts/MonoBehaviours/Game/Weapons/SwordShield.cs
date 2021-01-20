using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NextOne
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptObj/Weapons/SwordShield", order = 1)]
    public class SwordShield : Weapon
    {

        public GameObject swordModel;
        public GameObject shieldModel;

        private GameObject sword = null;
        private GameObject shield = null;
        
        ///</summary> Creates the bow and parent it to the left Hand </summary>
        public override void Create(Animator animator, Transform rightHand, Transform leftHand)
        {
            // Set the animator
            animator.runtimeAnimatorController = this.weaponAnimator;

            this.sword = GameObject.Instantiate(swordModel, rightHand);
            this.shield = GameObject.Instantiate(shieldModel, leftHand);
            
        }
    }
}