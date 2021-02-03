using System;
using UnityEngine;

namespace NextOne
{
    public class MeleeBasicAttackBehavior : BaseSkillBehavior
    {
        private Transform FirePoint;
        private SkillUseParams UseParams;
        private bool Cast = false;

        public override void Use(SkillUseParams _useParams)
        {
            if (!CanCast())
                return;
            base.Use(_useParams);

            UseParams = _useParams;
            Randomize();
        }

        protected override void Update()
        {
            //TODO: Overlap for player ! :)
            base.Update();
            if (!SkillInUse) return;

            if (!SourceController.HasAnimatorPlaying(GetRandomAnimationName(), 0)) return;

            //Looking for effective weapon changed during animation
            if (SourceController.IsAnimationLastAtLeast(GetRandomEffectiveTime(), 0) && !Cast)
            {
                Debug.Log("Weapon Changed in: " + this.GetInstanceID());
                OnEffectiveUse();
            }

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
            Cast = false;
        }

        protected override void OnInitialization()
        {
            SourceController = GetComponent<PlayerController>();
            if (!SourceController)
                SourceController = GetComponent<EnemyController>();
            FirePoint = SourceController.GetCastPoint(ECastPoint.Enemy)[0];
            if (!FirePoint)
                throw new Exception();
            Cast = false;
        }

        protected override void OnEffectiveUse()
        {
            MeleeSkillData meleeSkillData = (MeleeSkillData) SkillData;

            if (meleeSkillData.CastVfx)
            {
                GameObject meleeCast = Instantiate(meleeSkillData.CastVfx, FirePoint.position, Quaternion.identity);
                Debug.Log("Melee Skill Instantiated");
                ParticleSystem ps = meleeCast.GetComponent<ParticleSystem>();
                if (ps)
                    Destroy(meleeCast, ps.main.duration);
                else
                {
                    ParticleSystem psChild = meleeCast.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(meleeCast, psChild.main.duration);
                }
            }

            //Cast Instantiation
            Cast = true;
        }
    }
}