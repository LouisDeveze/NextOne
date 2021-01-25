using UnityEngine;

namespace NextOne
{
    [CreateAssetMenu(fileName = "MorphingSkill", menuName = "Next One/Skills/Morphing Skill")]
    public class MorphingSkillData : SkillData
    {
        [SerializeField]
        private float AnimationEffectiveChangeTime;

        public override ISkill AttachComponentTo(GameObject _gameObjectToAttachTo)
        {
            var behaviorComponent = _gameObjectToAttachTo.AddComponent<MorphingSkillBehavior>();
            behaviorComponent.SetData(this);
            Behavior = behaviorComponent;

            return Behavior;
        }

        public float EffectiveChangeTime => AnimationEffectiveChangeTime;
    }
}