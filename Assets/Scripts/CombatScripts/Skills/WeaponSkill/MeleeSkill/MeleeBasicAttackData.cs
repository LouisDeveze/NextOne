using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills.WeaponSkill.MeleeSkill
{
    [CreateAssetMenu(fileName = "MeleeSkill", menuName = "Next One/Skills/Melee Skill")]
    public class MeleeBasicAttackData : SkillData
    {
        [SerializeField] private int SkillDamage;

        public override ISkill AttachComponentTo(GameObject _gameObjectToAttachTo)
        {
            var behaviorComponent = _gameObjectToAttachTo.AddComponent<MeleeBasicAttackBehavior>();
            behaviorComponent.SetData(this);
            Behavior = behaviorComponent;

            return Behavior;
        }

        public int Damage => SkillDamage;
    }
}