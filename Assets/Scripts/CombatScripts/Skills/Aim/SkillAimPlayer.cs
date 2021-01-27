using UnityEngine;

namespace NextOne
{
    [CreateAssetMenu(fileName = "SkillAimPlayer", menuName = "Next One/Aim/Skill Aim Player")]
    public class SkillAimPlayer : SkillAim
    {
        public override void GetTarget(SkillUseParams _useParams)
        {
            _useParams.Target = new SelfTarget(_useParams.Origin);
        }
    }
}