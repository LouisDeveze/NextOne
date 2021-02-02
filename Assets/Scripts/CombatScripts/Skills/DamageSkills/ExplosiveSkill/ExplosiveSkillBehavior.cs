using System;
using UnityEngine;

namespace NextOne
{
    public class ExplosiveSkillBehavior : BaseSkillBehavior
    {
        private Transform FirePoint;
        private SkillUseParams UseParams;
        private bool ProjectileShoot;

        protected override void OnInitialization()
        {
            SourceController = GetComponent<PlayerController>();
            if (!SourceController)
                SourceController = GetComponent<EnemyController>();
            ProjectileShoot = false;
            FirePoint = SourceController.GetCastPoint(ECastPoint.Player)[0];
            if (!FirePoint)
                throw new Exception();
        }

        protected override void Update()
        {
            base.Update();

            if (!SkillInUse) return;
            if (!SourceController.HasAnimatorPlaying(GetRandomAnimationName(), 0)) return;

            if (SourceController.IsAnimationLastAtLeast(GetRandomEffectiveTime(), 0) && !ProjectileShoot)
            {
                Debug.Log("Explosive Shoot in: " + this.GetInstanceID());
                ProjectileShoot = true;
                OnEffectiveUse();
            }

            if (!SourceController.IsAnimationLastAtLeast(GetRandomAnimationTime(), 0)) return;
            Debug.Log("Explosive Skill Ended in: " + this.GetInstanceID());
            OnEffectEnd();
        }

        public override void Use(SkillUseParams _useParams)
        {
            if (!CanCast())
                return;
            base.Use(_useParams);
           

            UseParams = _useParams;
            Randomize();
            
            Debug.Log("Explosive Skill Used in: " + this.GetInstanceID());
            OnEffectStart();
        }

        private void ShootProjectile()
        {
            SkillData.Aim.GetTarget(UseParams);

            if (!(UseParams.Target is ForwardTarget direction))
            {
                Debug.Log("Forward Aiming Not Attached: " + this.GetInstanceID());
                return;
            }

            ExplosiveSkillData explosiveSkillData = (ExplosiveSkillData) this.SkillData;

            GameObject newExplosive = Instantiate(explosiveSkillData.ProjectilePrefab, FirePoint.position,
                Quaternion.identity);
            newExplosive.AddComponent<Explosive>();

            Debug.Log("New Explosive: " + newExplosive.GetInstanceID() + " instantiated in: "
                      + this.GetInstanceID());

            Explosive explosive = newExplosive.GetComponent<Explosive>();
            explosive.Source = UseParams.Origin;
            explosive.SourceID = this.GetInstanceID();
            explosive.Damage = explosiveSkillData.Damage;
            explosive.Delay = explosiveSkillData.ExplosiveDelay;
            explosive.ExplosionAim = explosiveSkillData.ExplosionTarget;
            explosive.EffectPrefab = explosiveSkillData.ExplosionEffect;
            explosive.gameObject.AddComponent<AudioSource>();
            explosive.Sfx = explosiveSkillData.ExplosionSfx;

            Debug.Log("Explosive: " + explosive.GetInstanceID() + " instantiated in: "
                      + this.GetInstanceID());

            //TODO: FIX POSITION
            newExplosive.GetComponent<Rigidbody>()
                .AddForce(direction.Direction * explosiveSkillData.ThrowForce);
        }

        protected override void OnEffectiveUse()
        {
            ShootProjectile();
        }

        protected override void OnEffectStart()
        {
            SourceController.CanMove(false);
            SourceController.SkillInUse = true;
            SkillInUse = true;
            SourceController.ResetTriggersAnimator();
            SourceController.SetTriggerAnimator(GetRandomAnimationName());
            Debug.Log("Explosive Skill Animation Triggered in: " +
                      this.GetInstanceID());
          
        }

        protected override void OnEffectEnd()
        {
            Debug.Log("Explosive Skill Ended: " +
                      this.GetInstanceID());
            SourceController.ResetTriggersAnimator();
            SourceController.CanMove(true);
            SourceController.SkillInUse = false;
            SkillInUse = false;
            ProjectileShoot = false;
        }
    }
}