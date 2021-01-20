using Assets.Scripts.Utility;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills.Aim
{
    [CreateAssetMenu(fileName = "NewSkillAimRaycast", menuName = "Next One/Aim/Skill Aim Raycast")]
    public class SkillAimRaycast : SkillAimPhysics
    {
        public float MaxDistance;

        public override Target GetTarget(GameObject _origin)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(_origin.transform.position, _origin.transform.forward, out raycastHit, MaxDistance, LayerMask))
            {
                var targetable = raycastHit.collider.GetComponent<Targetable>();
                if (targetable != null) return new Vector3Target(_origin,targetable.transform);
            }

            return null;
        }
    }
}