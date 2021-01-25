using UnityEngine;

namespace NextOne
{
    public class RangedSkillBehavior : BaseSkillBehavior
    {
        public override void Use(SkillUseParams _useParams)
        {
            //Get The Direction Where To Shoot !
            SkillData.Aim.GetTarget(_useParams);

            if (!(_useParams.Target is ForwardTarget direction))
            {
                Debug.Log("No Forward Target");
                return;
            }

            RangedSkillData rangedSkillData = (RangedSkillData) this.SkillData;

            GameObject newProjectile = Instantiate(rangedSkillData.ProjectilePrefab,
                _useParams.Origin.transform.position, Quaternion.identity);
            newProjectile.AddComponent<Projectile>();

            //TODO: Use FirePoint
            Projectile projectile = newProjectile.GetComponent<Projectile>();
            projectile.Source = _useParams.Origin;
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

            if (rangedSkillData.CastSfx)
            {
                projectile.PlaySfx(rangedSkillData.CastSfx);
            }

            //Set Motion
            //projectile.GetComponent<Rigidbody>().AddForce(direction.Direction * RangedSkillData.Velocity);
        }

        protected override void OnEffectStart()
        {
        }

        protected override void OnEffectEnd()
        {
        }

        protected override void OnInitialization()
        {
        }
    }
}