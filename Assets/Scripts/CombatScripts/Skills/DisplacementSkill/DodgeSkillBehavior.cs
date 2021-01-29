using UnityEngine;
using Random = System.Random;

namespace NextOne
{
    public class DodgeSkillBehavior : BaseSkillBehavior
    {
        protected override void Update()
        {
            //Update CD
            base.Update();

            // If skill is not in use
            if (!SkillInUse) return;
            // If Animation currently running has not yet transitioned to the Skill One
            if (!SourceController.HasAnimatorPlaying(GetRandomAnimationName(), 0)) return;
            // Checking Animation Ended
            if (!SourceController.IsAnimationLastAtLeast(GetRandomAnimationTime(), 0)) return;
            OnEffectEnd();
        }

        public override void Use(SkillUseParams _useParams)
        {
            if (!CanCast())
                return;

            base.Use(_useParams);

            SkillInUse = true;
            SourceController.SkillInUse = true;
            SourceController.CanMove(false);
            Randomize();
            OnEffectStart();
        }

        protected override void OnEffectStart()
        {
            SourceController.ResetTriggersAnimator();
            SourceController.SetTriggerAnimator(GetRandomAnimationName());
        }

        protected override void OnEffectEnd()
        {
            SourceController.CanMove(true);
            SourceController.ResetTriggersAnimator();
            SourceController.SkillInUse = false;
            SkillInUse = false;
        }

        protected override void OnInitialization()
        {
            SourceController = GetComponent<PlayerController>();
        }

        protected override void OnEffectiveUse()
        {
            throw new System.NotImplementedException();
        }
    }
}