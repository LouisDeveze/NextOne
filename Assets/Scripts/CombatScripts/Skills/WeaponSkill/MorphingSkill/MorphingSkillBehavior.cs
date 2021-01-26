using UnityEngine;
using UnityEngine.PlayerLoop;

namespace NextOne
{
    public class MorphingSkillBehavior : BaseSkillBehavior
    {
        private bool WeaponChanged = false;

        //CHECK INSTANTATION -> Attach GameObject
        void Update()
        {
            //If Skill In Use
            if (!SkillInUse) return;
            //If Current Skill Animation
            if (!Player.hasAnimatorPlaying(SkillData.AnimationName, 0)) return;

            //Looking for effective weapon changed during animation
            if (Player.IsAnimationLastAtLeast(SkillData.EffectiveChangeTime, 0) && !WeaponChanged)
            {
                Debug.Log("Weapon Changed in: " + this.GetInstanceID());
                OnEffectiveUse();
            }

            //If Weapon Newly Changed
            if (!WeaponChanged) return;
            //If Change Animation Completed
            if (!Player.IsAnimationLastAtLeast(SkillData.AnimationTime, 0)) return;
            Debug.Log("Morphing Skill Ended in: " + this.GetInstanceID());
            //End Animation
            OnEffectEnd();
        }


        public override void Use(SkillUseParams _useParams)
        {
            //TODO: Change for an actual cooldown
            // If the Animator is still playing animation from previous weapon
            if (Player.hasAnimatorPlaying(this.SkillData.AnimationName, 0))
            {
                Player.SkillInUse = false;
                SkillInUse = false;

                return;
            }

            SkillInUse = Player.AttemptWeaponChange();

            if (!SkillInUse) return;

            Debug.Log("Morphing Skill Used in: " + this.GetInstanceID());
            Player.CanMove(false);
            OnEffectStart();
        }

        protected override void OnInitialization()
        {
            Debug.Log("Initialization in: " + this.GetInstanceID());
            Player = GetComponent<PlayerController>();
            SkillInUse = Player.SkillInUse;
            WeaponChanged = this.SkillInUse;
            //If Instantiated while Skill In use 
            // -> The player just changed weapon
        }

        protected override void OnEffectStart()
        {
            //Animation Related
            Player.ResetTriggersAnimator();
            Player.SetTriggerAnimator(SkillData.AnimationName);
            Debug.Log("Morphing Animation Triggered in: " + this.GetInstanceID());
            SkillInUse = true;
            Player.SkillInUse = true;
        }

        private void OnEffectiveUse()
        {
            Player.ChangeWeapon();
            WeaponChanged = true;
        }

        protected override void OnEffectEnd()
        {
            Player.ResetTriggersAnimator();
            Player.CanMove(true);
            WeaponChanged = false;
            SkillInUse = false;
            Player.SkillInUse = false;
        }
    }
}