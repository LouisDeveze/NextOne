using UnityEngine;

namespace NextOne
{
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