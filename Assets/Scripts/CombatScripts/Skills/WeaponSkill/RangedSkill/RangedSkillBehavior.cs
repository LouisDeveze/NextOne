using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills.WeaponSkill.RangedSkill
{
    public class RangedSkillBehavior : MonoBehaviour, ISkill
    {
        private RangedSkillData RangedSkillData = null;

        void Start()
        {
        }

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

            if (RangedSkillData.MuzzlePrefab)
            {
                var MuzzleVFX = Instantiate(RangedSkillData.MuzzlePrefab,
                    _useParams.Origin.transform.position, Quaternion.identity);
                var ps = MuzzleVFX.GetComponent<ParticleSystem>();
                if (ps)
                    Destroy(MuzzleVFX, ps.main.duration);
                else
                {
                    var psChild = MuzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(MuzzleVFX, psChild.main.duration);
                }
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