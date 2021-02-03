using UnityEngine;

namespace NextOne
{
    [CreateAssetMenu(fileName = "Enemy Skill Trigger", menuName = "Next One/Skills/Triggers/Enemy Skill Trigger")]
    public class EnemySkillTrigger : SkillTrigger
    {

        public override bool IsTriggered(SkillUseParams _distanceToPlayer)
        {
            return _distanceToPlayer.DistanceToPlayer <= _distanceToPlayer.DetectRange;
        }

        public override string KeyCodeToString()
        {
            throw new System.NotImplementedException();
        }
        
    }
}