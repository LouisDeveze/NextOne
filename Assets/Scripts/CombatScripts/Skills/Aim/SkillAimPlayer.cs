using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills.Aim
{
    [CreateAssetMenu(fileName = "SkillAimPlayer", menuName = "Next One/Aim/Skill Aim Player")]
    public class SkillAimPlayer : SkillAim
    {
        public override Target GetTarget(GameObject _origin)
        {
            return new SelfTarget(_origin);
        }
    }
}