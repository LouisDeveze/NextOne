using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills.WeaponSkill.MorphingSkill
{
    [CreateAssetMenu(fileName = "MorphingSkill", menuName = "Next One/Skills/Morphing Skill")]
    public class MorphingSkillData : SkillData
    {
        public override ISkill AttachComponentTo(GameObject _gameObjectToAttachTo)
        {
            var behaviorComponent = _gameObjectToAttachTo.AddComponent<MorphingSkillBehavior>();
            behaviorComponent.SetData(this);
            Behavior = behaviorComponent;

            return Behavior;
        }
    }
}