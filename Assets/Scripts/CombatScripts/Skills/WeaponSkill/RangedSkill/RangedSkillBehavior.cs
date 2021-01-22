using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills.WeaponSkill.RangedSkill
{
    public class RangedSkillBehavior : MonoBehaviour, ISkill
    {
        private RangedSkillData RangedSkillData = null;

        public void SetData(RangedSkillData _rangedSkillData)
        {
            this.RangedSkillData = _rangedSkillData;
        }

        public void Use(SkillUseParams _useParams)
        {
            //Get The Direction Where To Shoot !
            RangedSkillData.Aim.GetTarget(_useParams);

            if (!(_useParams.Target is ForwardTarget direction))
            {
                Debug.Log("No Forward Target");
                return;
            }

            GameObject newProjectile = Instantiate(RangedSkillData.ProjectilePrefab,
                _useParams.Origin.transform.position, Quaternion.identity);
            newProjectile.AddComponent<Projectile>();

            //TODO: Use FirePoint
            Projectile projectile = newProjectile.GetComponent<Projectile>();
            projectile.Source = _useParams.Origin;
            projectile.Damage = RangedSkillData.Damage;
            projectile.DestroyDelay = RangedSkillData.Delay;
            projectile.Hit = RangedSkillData.HitPrefab;
            projectile.Trails = RangedSkillData.TrailsPrefab;
            projectile.MaxCollision = RangedSkillData.MaxCollision;
            projectile.gameObject.AddComponent<AudioSource>();
            projectile.HitSfx = RangedSkillData.HitSfx;


            if (RangedSkillData.MuzzlePrefab)
            {
                var muzzleVfx = Instantiate(RangedSkillData.MuzzlePrefab,
                    _useParams.Origin.transform.position, Quaternion.identity);
                var ps = muzzleVfx.GetComponent<ParticleSystem>();
                if (ps)
                    Destroy(muzzleVfx, ps.main.duration);
                else
                {
                    var psChild = muzzleVfx.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(muzzleVfx, psChild.main.duration);
                }
            }

            if (RangedSkillData.CastSfx)
            {
                projectile.PlaySfx(RangedSkillData.CastSfx);
            }

            //Set Motion
            projectile.GetComponent<Rigidbody>().AddForce(direction.Direction * RangedSkillData.Velocity);
        }

        public void Detach()
        {
            Destroy(this);
        }

        private void PlayEffect()
        {
        }
    }
}