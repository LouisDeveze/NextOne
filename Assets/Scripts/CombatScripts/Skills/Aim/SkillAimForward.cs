using Assets.Scripts.CombatScripts.Enemies;
using Assets.Scripts.Utility;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills.Aim
{
    [CreateAssetMenu(fileName = "NewSkillAimForward", menuName = "Next One/Aim/Skill Aim Forward")]
    public class SkillAimForward : SkillAimParams
    {
        public float MaxDistance;

        public override void GetTarget(SkillUseParams _useParams)
        {
            _useParams.Target = new ForwardTarget(_useParams.Origin);
        }
    }
}