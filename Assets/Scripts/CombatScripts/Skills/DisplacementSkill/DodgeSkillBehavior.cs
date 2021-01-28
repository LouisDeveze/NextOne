using UnityEngine;
using Random = System.Random;

namespace NextOne
{
    public class DodgeSkillBehavior : BaseSkillBehavior
    {
        void Update()
        {
            // If skill is not in use
            if (!SkillInUse) return;
            // If Animation currently running has not yet transitioned to the Skill One
            if (!Player.hasAnimatorPlaying(GetRandomAnimationName(), 0)) return;
            // Checking Animation Ended
            if (!Player.IsAnimationLastAtLeast(GetRandomAnimationTime(), 0)) return;
            OnEffectEnd();
        }

        public override void Use(SkillUseParams _useParams)
        {
            SkillInUse = true;
            Player.CanMove(false);
            Randomize();
            OnEffectStart();
        }

        protected override void OnEffectStart()
        {
            Player.ResetTriggersAnimator();
            Player.SetTriggerAnimator(GetRandomAnimationName());
        }

        protected override void OnEffectEnd()
        {
            Player.CanMove(true);
            Player.ResetTriggersAnimator();
            Player.SkillInUse = false;
            SkillInUse = false;
        }

        protected override void OnInitialization()
        {
            Player = GetComponent<PlayerController>();
        }

        protected override void OnEffectiveUse()
        {
            throw new System.NotImplementedException();
        }
    }
}