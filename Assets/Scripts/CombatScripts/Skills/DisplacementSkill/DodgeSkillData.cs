using UnityEngine;

namespace NextOne
{
    [CreateAssetMenu(fileName = "DodgeSkill", menuName = "Next One/Skills/Dodge Skill")]
    public class DodgeSkillData : SkillData
    {
        public override ISkill AttachComponentTo(GameObject _gameObjectToAttachTo)
        {
            var behaviorComponent = _gameObjectToAttachTo.AddComponent<DodgeSkillBehavior>();
            behaviorComponent.SetData(this);
            Behavior = behaviorComponent;

            return Behavior;
        }
    }
}