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

            //Use FirePoint
            Projectile projectile = newProjectile.GetComponent<Projectile>();
            projectile.Source = _useParams.Origin;
            projectile.Damage = RangedSkillData.Damage;
            projectile.DestroyDelay = RangedSkillData.Delay;

            /*Projectile projectile = new Projectile(gameObject, RangedSkillData.Damage,
                RangedSkillData.Delay);*/

            //Set Motion
            projectile.GetComponent<Rigidbody>().AddForce(direction.Direction * RangedSkillData.Velocity);
            //projectile.GetComponent<Rigidbody>().velocity = direction.Direction * RangedSkillData.Velocity;
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