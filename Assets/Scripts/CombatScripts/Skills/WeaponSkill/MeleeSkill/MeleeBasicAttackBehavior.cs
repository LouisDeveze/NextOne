using Assets.Scripts.CombatScripts.Enemies;
using Assets.Scripts.CombatScripts.Skills.Aim;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills.WeaponSkill.MeleeSkill
{
    public class MeleeBasicAttackBehavior : MonoBehaviour, ISkill
    {
        private MeleeBasicAttackData MeleeBasicAttackData = null;

        void Start()
        {
        }

        public void SetData(MeleeBasicAttackData _meleeBasicAttackData)
        {
            this.MeleeBasicAttackData = _meleeBasicAttackData;
        }

        public void Use(SkillUseParams _useParams)
        {
            MeleeBasicAttackData.Aim.GetTarget(_useParams);

            if (!(_useParams.Target is EnemyTarget targets))
            {
                Debug.Log("no enemy");
                return;
            }

            foreach (var target in targets.Enemies)
            {
                Debug.Log(target.EnemyData.Name + " targeted");
                target.TakeDamage(MeleeBasicAttackData.Damage);
                Debug.Log(" Take " + MeleeBasicAttackData.Damage +
                          " has now " + target.EnemyHealth);
            }
        }

        public void Detach()
        {
            Destroy(this);
        }

        private void PlayEffect()
        {
            //TODO: Implement VFX
        }
    }
}