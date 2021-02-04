using System;
using Assets.Scripts.Data;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace NextOne
{
    public class MeleeBasicAttackBehavior : BaseSkillBehavior
    {
        private Transform FirePoint;
        private SkillUseParams UseParams;
        private bool Cast = false;

        public override void Use(SkillUseParams _useParams)
        {
            Debug.Log("Melee Skill Use: " + SkillData.Name + " - " + this.GetInstanceID());

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

            //Looking for effective weapon changed during animation
            if (SourceController.IsAnimationLastAtLeast(GetRandomEffectiveTime(), 0) && !Cast)
            {
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
            if (SourceController is PlayerController playerController)
                playerController.ActiveWeaponTrigger(true);
            else ((EnemyController) SourceController).ActiveClawsTrigger(true);


            SourceController.SetTriggerAnimator(GetRandomAnimationName());
            Debug.Log("Melee Basic Attack in: " + SkillData.Name + " - " + this.GetInstanceID());

            /////FROM ON EFFECTIVE USE
            /*   MeleeSkillData meleeSkillData = (MeleeSkillData) SkillData;
   
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
               }*/
        }

        protected override void OnEffectEnd()
        {
            SourceController.ResetTriggersAnimator();
            SourceController.CanMove(true);
            SourceController.SkillInUse = false;
            SkillInUse = false;
            Cast = false;
            
            if (SourceController is PlayerController playerController)
                playerController.ActiveWeaponTrigger(false);
            else ((EnemyController) SourceController).ActiveClawsTrigger(false);
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
/*
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
            }*/

            //Cast Instantiation
            Cast = true;
        }
    }
}