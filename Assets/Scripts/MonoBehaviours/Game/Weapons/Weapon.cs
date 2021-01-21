using UnityEngine;

namespace NextOne
{

    public class Weapon : ScriptableObject
    {

        // J'ai laissé uniquement ce que j'utilise Clément
        public AnimatorOverrideController weaponAnimator;

        // Angle in degrees before running animation becomes strafe
        private float tresholdStrafe = .5f;

        /// TO ADD UI
        // This function should be overrided to  instantiate the prefabs of the Weapon
        public virtual void Create(Animator animator, Transform rightHand, Transform leftHand) {
            // Set the animator
            animator.runtimeAnimatorController = this.weaponAnimator;
        }

        // This function should be overrided to destroy the prebabs of the weapon
        public virtual void Destroy() { }

        public void AnimateMovement(Animator animator, GameObject model, Vector3 movement, float angle) {

            string trigger = Animations.Idle; ;

            // If movement in right is superior to the Strafe treshold, Strafe Right
            if (movement.x > tresholdStrafe) { trigger = Animations.StrafeRight; }
            // If movement in Left is superior to the Strafe treshold, Strafe Left
            else if (movement.x < -tresholdStrafe) { trigger = Animations.StrafeLeft; }
            // Else check the movement in Z to now if player is running backward or frontward
            else if (movement.z > 0) { trigger = Animations.RunFront; }
            // Else check the movement in Z to now if player is running backward or frontward
            else if (movement.z < 0) { trigger = Animations.RunBack; }
            // Else if idle and turning a lot
            else if (movement.magnitude == 0 && angle > 10) { trigger = Animations.TurnLeft; }
            else if (movement.magnitude == 0 && angle < -10) { trigger = Animations.TurnRight; }
            // Idle triggered when there are no movement
            else { trigger = Animations.Idle; }

            Animations.ResetTriggers(animator);
            animator.SetTrigger(trigger);

            
        }
    }

    
}
