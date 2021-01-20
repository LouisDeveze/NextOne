using Assets.Scripts.Utility;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills.Aim
{
    [CreateAssetMenu(fileName = "NewSkillAimOverlap", menuName = "Next One/Aim/Skill Aim Overlap")]
    public class SkillAimOverlap : SkillAimPhysics
    {
        public float radius;

        public override Target GetTarget(GameObject _origin)
        {
            var colliders = Physics.OverlapSphere(_origin.transform.position, radius, LayerMask);
            foreach (var collider in colliders)
            {
                var targetable = collider.GetComponent<Targetable>();
                if (targetable != null) return new Vector3Target(_origin,targetable.transform);
            }

            return null;
        }
    }
}