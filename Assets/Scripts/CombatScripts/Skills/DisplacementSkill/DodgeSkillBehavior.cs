using UnityEngine;

namespace NextOne
{
    public class DodgeSkillBehavior : BaseSkillBehavior
    {
        void Update()
        {
            if (SkillInUse)
                Framecount++;
            if (!Player.IsAnimationEnded(SkillData.AnimationTime) || !SkillInUse || !(Framecount > 1)) return;
            OnEffectEnd();
        }

        public override void Use(SkillUseParams _useParams)
        {
            Framecount = 0;
            SkillInUse = true;
            Player.CanMove(false);
            OnEffectStart();
        }

        protected override void OnEffectStart()
        {
            Player.ResetTriggersAnimator();
            Player.SetTriggerAnimator(SkillData.AnimationName);
        }

        protected override void OnEffectEnd()
        {
            Player.CanMove(true);
            Player.ResetTriggersAnimator();
            Player.SkillInUse = false;
            SkillInUse = false;
        }
    }
}