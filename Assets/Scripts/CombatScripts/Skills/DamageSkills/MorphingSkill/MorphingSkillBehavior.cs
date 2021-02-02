using UnityEngine;
using UnityEngine.PlayerLoop;

namespace NextOne
{
    public class MorphingSkillBehavior : BaseSkillBehavior
    {
        private bool WeaponChanged = false;

        //CHECK INSTANTATION -> Attach GameObject
        protected override void Update()
        {
            base.Update();

            //If Skill In Use
            if (!SkillInUse) return;
            //If Current Skill Animation
            if (!SourceController.HasAnimatorPlaying(GetRandomAnimationName(), 0)) return;

            //Looking for effective weapon changed during animation
            if (SourceController.IsAnimationLastAtLeast(GetRandomEffectiveTime(), 0) && !WeaponChanged)
            {
                Debug.Log("Weapon Changed in: " + this.GetInstanceID());
                OnEffectiveUse();
            }

            //If Weapon Newly Changed
            if (!WeaponChanged) return;
            //If Change Animation Completed
            if (!SourceController.IsAnimationLastAtLeast(GetRandomAnimationTime(), 0)) return;
            Debug.Log("Morphing Skill Ended in: " + this.GetInstanceID());
            //End Animation
            OnEffectEnd();
        }


        public override void Use(SkillUseParams _useParams)
        {
            if (!CanCast())
                return;

            base.Use(_useParams);

            Randomize();
            //TODO: Change for an actual cooldown
            // If the Animator is still playing animation from previous weapon
            if (SourceController.HasAnimatorPlaying(GetRandomAnimationName(), 0))
            {
                SourceController.SkillInUse = false;
                SkillInUse = false;
                return;
            }

            SkillInUse = ((PlayerController) SourceController).AttemptWeaponChange();

            if (!SkillInUse) return;

            Debug.Log("Morphing Skill Used in: " + this.GetInstanceID());
            SourceController.CanMove(false);
            OnEffectStart();
        }

        protected override void OnInitialization()
        {
            Debug.Log("Morphing Skill Initialization in: " + this.GetInstanceID());
            SourceController = GetComponent<PlayerController>();
            SkillInUse = SourceController.SkillInUse;
            WeaponChanged = this.SkillInUse;
            //If Instantiated while Skill In use 
            // -> The player just changed weapon
        }

        protected override void OnEffectStart()
        {
            //Animation Related
            SourceController.ResetTriggersAnimator();
            SourceController.SetTriggerAnimator(GetRandomAnimationName());
            Debug.Log("Morphing Animation Triggered in: " + this.GetInstanceID());
            SkillInUse = true;
            SourceController.SkillInUse = true;
        }

        protected override void OnEffectiveUse()
        {
            ((PlayerController) SourceController).ChangeWeapon();
            WeaponChanged = true;
        }

        protected override void OnEffectEnd()
        {
            SourceController.ResetTriggersAnimator();
            SourceController.CanMove(true);
            WeaponChanged = false;
            SkillInUse = false;
            SourceController.SkillInUse = false;
        }
    }
}