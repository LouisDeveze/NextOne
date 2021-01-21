using Assets.Scripts.Utility;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills.Aim
{
    [CreateAssetMenu(fileName = "NewSkillAimOverlap", menuName = "Next One/Aim/Skill Aim Overlap")]
    public class SkillAimOverlap : SkillAimParams
    {
        public float radius;

        public override void GetTarget(SkillUseParams _params)
        {
            var colliders = Physics.OverlapSphere(_params.Origin.transform.position, radius, LayerMask);
            foreach (var collider in colliders)
            {
                var targetable = collider.GetComponent<Targetable>();
                if (targetable != null)
                    _params.Target = new Vector3Target(_params.Origin, targetable.transform);
            }
        }
    }
}