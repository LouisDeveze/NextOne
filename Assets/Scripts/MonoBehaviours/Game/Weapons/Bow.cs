using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NextOne
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptObj/Weapons/Bow", order = 1)]
    public class Bow : Weapon
    {

        public GameObject bowModel;

        private GameObject bow = null;


        /// <summary>
        /// Creates the bow and parent it to the left Hand
        /// </summary>
        /// <param name="rightHand"></param>
        /// <param name="leftHand"></param>
        /// <returns>Hte prefab of the Bow</returns>
        public override void Create(Animator animator, Transform rightHand, Transform leftHand)
        {
            animator.runtimeAnimatorController = this.weaponAnimator;

            this.bow = GameObject.Instantiate(bowModel, leftHand);

            // Resolve Tranform Problems
            this.bow.transform.localPosition = new Vector3(-0.0001f, 0.001f, 0.0008f);
            this.bow.transform.localEulerAngles = new Vector3(98,0,0);
            this.bow.transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
        }
    }
}