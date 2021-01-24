using UnityEngine;
using UnityEngine.PlayerLoop;

namespace NextOne
{
    public class MorphingSkillBehavior : BaseSkillBehavior
    {
        void Update()
        {
            // If skill is not in use
            if (!SkillInUse) return;
            // If Animation currently running has not yet transitioned to the Skill One
            if (!Player.hasAnimatorPlaying(SkillData.AnimationName, 0)) return;

            //if(Player.IsAnimationLastAtLeast(0.5f,0))
            //Player.ChangeWeapon();

            // Checking Animation Ended
            if (!Player.IsAnimationLastAtLeast(SkillData.AnimationTime, 0)) return;
            OnEffectEnd();
        }

        public override void Use(SkillUseParams _useParams)
        {
            // If the Animator is still playing animation from previous weapon
            if (Player.hasAnimatorPlaying(this.SkillData.AnimationName, 0))
            {
                Player.SkillInUse = false;
                SkillInUse = false;
                
                return;
            }

            SkillInUse = Player.AttemptWeaponChange();

            if (!SkillInUse) return;
            
            Player.CanMove(false);
            OnEffectStart();
        }


        protected override void OnEffectStart()
        {
            //Animation Related
            Player.ResetTriggersAnimator();
            Player.SetTriggerAnimator(SkillData.AnimationName);

        }

        protected override void OnEffectEnd()
        {
            Player.CanMove(true);
            Player.ResetTriggersAnimator();
            Player.ChangeWeapon();
            SkillInUse = false;
            Player.SkillInUse = false;
        }
    }
}