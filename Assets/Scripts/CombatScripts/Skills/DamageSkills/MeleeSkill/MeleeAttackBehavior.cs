using UnityEngine;

namespace NextOne
{
    public class MeleeAttackBehavior : BaseSkillBehavior
    {
        private SkillUseParams UseParams;

        public override void Use(SkillUseParams _useParams)
        {
            if (!CanCast())
                return;
            base.Use(_useParams);


            UseParams = _useParams;
            Randomize();
            OnEffectStart();
        }

        protected override void Update()
        {
            base.Update();
            if (!SkillInUse) return;

            if (!SourceController.HasAnimatorPlaying(GetRandomAnimationName(), 0)) return;

            if (!SourceController.IsAnimationLastAtLeast(GetRandomAnimationTime(), 0)) return;
            Debug.Log("Melee Skill Ended in: " + SkillData.Name + " - " + this.GetInstanceID());
            OnEffectEnd();
        }

        protected override void OnEffectStart()
        {
            SourceController.SkillInUse = true;
            SkillInUse = true;
            SourceController.CanMove(false);
            SourceController.ResetTriggersAnimator();
            ((PlayerController) SourceController).ActiveWeaponTrigger(true);
            SourceController.SetTriggerAnimator(GetRandomAnimationName());
            Debug.Log("Melee Basic Attack in: " + SkillData.Name + " - " + this.GetInstanceID());
        }

        protected override void OnEffectEnd()
        {
            SourceController.ResetTriggersAnimator();
            SourceController.CanMove(true);
            SourceController.SkillInUse = false;
            SkillInUse = false;
        }

        protected override void OnInitialization()
        {
            SourceController = GetComponent<PlayerController>();
            if (!SourceController)
                SourceController = GetComponent<EnemyController>();
        }

        protected override void OnEffectiveUse()
        {
            //NOTHING
        }
    }
}