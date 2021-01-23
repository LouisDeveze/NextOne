using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NextOne
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptObj/Weapons/SwordShield", order = 1)]
    public class SwordShield : MonoBehaviour
    {

        public GameObject swordModel;
        public GameObject shieldModel;

        private GameObject sword = null;
        private GameObject shield = null;
        
        ///<summary> Creates the sword / shield and parent it to the left Hand </summary>
        public  void Create(Animator animator, Transform rightHand, Transform leftHand)
        {
            Create(animator, rightHand, leftHand);

            this.sword = GameObject.Instantiate(swordModel, rightHand);
            this.shield = GameObject.Instantiate(shieldModel, leftHand);
            
        }

        ///<summary> Destroys the game Objects </summary>
        public  void Destroy()
        {
            Destroy(sword);
            Destroy(shield);
        }
    }
}