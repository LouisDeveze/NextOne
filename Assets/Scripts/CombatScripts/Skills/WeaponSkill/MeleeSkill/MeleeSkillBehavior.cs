using UnityEngine;

namespace NextOne
{
    public class MeleeBasicAttackBehavior : BaseSkillBehavior
    {
        public override void Use(SkillUseParams _useParams)
        {
            if (!CanCast())
                return;
            base.Use(_useParams);
     
            
            Randomize();
            MeleeSkillData meleeBasicAttackData = (MeleeSkillData) this.SkillData;

            meleeBasicAttackData.Aim.GetTarget(_useParams);

            if (!(_useParams.Target is EnemyTarget targets))
            {
                Debug.Log("no enemy");
                return;
            }

            foreach (var target in targets.Enemies)
            {
                Debug.Log(target.EnemyData.Name + " targeted");
                target.TakeDamage(meleeBasicAttackData.Damage);
                Debug.Log(" Take " + meleeBasicAttackData.Damage +
                          " has now " + target.EnemyHealth);
            }
        }

        protected override void OnEffectStart()
        {
            //TODO: Implement VFX
        }

        protected override void OnEffectEnd()
        {
        }

        protected override void OnInitialization()
        {
        }

        protected override void OnEffectiveUse()
        {
            throw new System.NotImplementedException();
        }
    }
}