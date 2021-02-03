using System;
using UnityEngine;
using UnityEngine.VFX;

namespace NextOne
{
    public class RangedSkillBehavior : BaseSkillBehavior
    {
        private Transform FirePoint;
        private SkillUseParams UseParams;
        private bool ProjectileShoot;

        protected override void Update()
        {
            base.Update();

            //If Skill In Use
            if (!SkillInUse) return;

            if (!SourceController.HasAnimatorPlaying(GetRandomAnimationName(), 0)) return;

            //Looking for effective time to shoot
            if (SourceController.IsAnimationLastAtLeast(GetRandomEffectiveTime(), 0) && !ProjectileShoot)
            {
                Debug.Log("Projectile Shoot in: " + this.GetInstanceID());
                ProjectileShoot = true;
                OnEffectiveUse();
            }

            if (!SourceController.IsAnimationLastAtLeast(GetRandomAnimationTime(), 0)) return;
            Debug.Log("Ranged Skill Ended in: " + this.GetInstanceID());
            OnEffectEnd();
        }

        public override void Use(SkillUseParams _useParams)
        {
            if (!CanCast())
                return;

            base.Use(_useParams);


            UseParams = _useParams;
            Randomize();
            OnEffectStart();
        }

        private void ShootProjectile()
        {
            //Get The Direction Where To Shoot !
            SkillData.Aim.GetTarget(UseParams);

            if (!(UseParams.Target is ForwardTarget direction))
            {
                Debug.Log("Forward Aiming Not Attached: " + this.GetInstanceID());
                return;
            }

            RangedSkillData rangedSkillData = (RangedSkillData) this.SkillData;


            GameObject newProjectile = Instantiate(rangedSkillData.ProjectilePrefab,
                FirePoint.position, Quaternion.identity);
            newProjectile.AddComponent<Projectile>();

            Debug.Log("New Projectile: " + newProjectile.GetInstanceID() + " instantiated in: "
                      + this.GetInstanceID());

            Projectile projectile = newProjectile.GetComponent<Projectile>();
            projectile.Source = UseParams.Origin;
            projectile.Damage = rangedSkillData.Damage;
            projectile.DestroyDelay = rangedSkillData.Delay;
            projectile.Hit = rangedSkillData.HitPrefab;
            projectile.Trails = rangedSkillData.TrailsPrefab;
            projectile.MaxCollision = rangedSkillData.MaxCollision;
            projectile.gameObject.AddComponent<AudioSource>();
            projectile.HitSfx = rangedSkillData.HitSfx;
            projectile.Velocity = rangedSkillData.Velocity;
            projectile.Accuracy = rangedSkillData.Accuracy;
            projectile.Direction = direction.Direction;

            if (rangedSkillData.MuzzlePrefab)
            {
                var muzzleVfx = Instantiate(rangedSkillData.MuzzlePrefab,
                    FirePoint.position, Quaternion.identity);
                Debug.Log("MuzzleFlash Instantiated");
                var ps = muzzleVfx.GetComponent<ParticleSystem>();
                if (ps)
                    Destroy(muzzleVfx, ps.main.duration);
                else
                {
                    var ve = muzzleVfx.GetComponent<VisualEffect>();
                    if (ve)
                    {
                       Destroy(muzzleVfx,.5f); 
                    }
                    else
                    {
                        
                        var psChild = muzzleVfx.transform.GetChild(0).GetComponent<ParticleSystem>();
                        Destroy(muzzleVfx, psChild.main.duration);
                    }
                    
                }
            }

            if (rangedSkillData.CastSfx)
            {
                projectile.PlaySfx(rangedSkillData.CastSfx);
            }
        }


        protected override void OnEffectiveUse()
        {
            ShootProjectile();
        }

        protected override void OnEffectStart()
        {
            SourceController.SkillInUse = true;
            SkillInUse = true;
            SourceController.CanMove(false);

            //Freeze Player & Play Animation 
            SourceController.ResetTriggersAnimator();
            SourceController.SetTriggerAnimator(GetRandomAnimationName());
            Debug.Log("Ranged Skill Animation Triggered in: " +
                      this.GetInstanceID());
        }

        protected override void OnEffectEnd()
        {
            SourceController.ResetTriggersAnimator();
            SourceController.CanMove(true);
            SourceController.SkillInUse = false;
            SkillInUse = false;
            ProjectileShoot = false;
        }

        protected override void OnInitialization()
        {
            SourceController = GetComponent<PlayerController>();
            if (!SourceController)
                SourceController = GetComponent<EnemyController>();
            ProjectileShoot = false;
            //Get First Cast Point
            //TODO: Handle when two ranged weapon, from which to shoot? =)
            FirePoint = SourceController.GetCastPoint(ECastPoint.Weapons)[0];
            if (!FirePoint)
                throw new Exception();
            //Should not occured !
        }
    }
}