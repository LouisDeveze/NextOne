﻿using System;
using UnityEngine;

namespace NextOne
{
    public class RangedSkillBehavior : BaseSkillBehavior
    {
        private Transform FirePoint;
        private SkillUseParams UseParams;
        private bool ProjectileShoot = false;

        void Update()
        {
            //If Skill In Use
            if (!SkillInUse) return;

            if (!Player.hasAnimatorPlaying(SkillData.AnimationName, 0)) return;

            //Looking for effective time to shoot
            if (Player.IsAnimationLastAtLeast(SkillData.EffectiveChangeTime, 0) && !ProjectileShoot)
            {
                Debug.Log("Projectile Shoot in: " + this.GetInstanceID());
                ProjectileShoot = true;
                OnEffectiveUse();
            }

            if (!Player.IsAnimationLastAtLeast(SkillData.AnimationTime, 0)) return;
            Debug.Log("Ranged Skill Ended in: " + this.GetInstanceID());
            OnEffectEnd();
        }

        public override void Use(SkillUseParams _useParams)
        {
            UseParams = _useParams;
            OnEffectStart();
        }

        private void OnEffectiveUse()
        {
            ShootProjectile();
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
                    var psChild = muzzleVfx.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(muzzleVfx, psChild.main.duration);
                }
            }

            if (rangedSkillData.CastSfx)
            {
                projectile.PlaySfx(rangedSkillData.CastSfx);
            }
        }

        protected override void OnEffectStart()
        {
            //Freeze Player & Play Animation 
            Player.ResetTriggersAnimator();
            Player.SetTriggerAnimator(SkillData.AnimationName);
            Debug.Log("Ranged Skill Animation Triggered in: " +
                      this.GetInstanceID());
            Player.SkillInUse = true;
            SkillInUse = true;
            Player.CanMove(false);
        }

        protected override void OnEffectEnd()
        {
            Player.ResetTriggersAnimator();
            Player.CanMove(true);
            Player.SkillInUse = false;
            SkillInUse = false;
            ProjectileShoot = false;
        }

        protected override void OnInitialization()
        {
            Player = GetComponent<PlayerController>();
            //Get First Cast Point
            //TODO: Handle when two ranged weapon, from which to shoot? =)
            FirePoint = Player.GetCastPoint()[0];
            if (!FirePoint)
                throw new Exception();
            //Should not occured !
        }
    }
}