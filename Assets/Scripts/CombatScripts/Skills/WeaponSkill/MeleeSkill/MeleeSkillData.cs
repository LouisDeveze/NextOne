using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills.WeaponSkill.MeleeSkill
{
    public class MeleeSkillData : SkillData
    {
        public override void AttachComponentTo(GameObject _gameObjectToAttachTo)
        {
            var behaviorComponent = _gameObjectToAttachTo.AddComponent<MeleeSkillBehavior>();
            behaviorComponent.SetData(this);
            Behavior = behaviorComponent;
        }
    }
}