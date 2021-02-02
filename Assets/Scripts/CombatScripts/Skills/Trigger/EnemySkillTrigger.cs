using UnityEngine;

namespace NextOne
{
    [CreateAssetMenu(fileName = "Enemy Skill Trigger", menuName = "Next One/Skills/Triggers/Enemy Skill Trigger")]
    public class EnemySkillTrigger : SkillTrigger
    {
        [SerializeField] private float TriggerDetectRange;

        public override bool IsTriggered(SkillUseParams _distanceToPlayer)
        {
            return _distanceToPlayer.DistanceToPlayer <= DetectRange;
        }

        public float DetectRange => TriggerDetectRange;
    }
}