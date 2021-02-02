using UnityEngine;

namespace NextOne
{
    [CreateAssetMenu(fileName = "MeleeAttack", menuName = "Next One/Skills/Melee Attack")]
    public class MeleeAttackData : SkillData
    {
        [SerializeField] private GameObject AttackCastPrefab;
        [SerializeField] private AudioClip AttackCastSfx;

        public override ISkill AttachComponentTo(GameObject _gameObjectToAttachTo)
        {
            var behaviorComponent = _gameObjectToAttachTo.AddComponent<MeleeAttackBehavior>();
            behaviorComponent.SetData(this);
            Behavior = behaviorComponent;

            return Behavior;
        }

        public GameObject CastPrefab => AttackCastPrefab;

        public AudioClip CastSfx => AttackCastSfx;
    }
}