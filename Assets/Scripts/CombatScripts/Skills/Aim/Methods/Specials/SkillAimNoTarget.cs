using UnityEngine;

namespace NextOne
{
    [CreateAssetMenu(fileName = "SkillAimNoTarget", menuName = "Next One/Aim/Skill Aim No Target")]
    public class SkillAimNoTarget : SkillAim
    {
        public override void GetTarget(SkillUseParams _params)
        {
            _params.Target = new NoTarget();
        }
    }
}