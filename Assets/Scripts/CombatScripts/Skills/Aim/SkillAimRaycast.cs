using Assets.Scripts.CombatScripts.Enemies;
using Assets.Scripts.Utility;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills.Aim
{
    [CreateAssetMenu(fileName = "NewSkillAimRaycast", menuName = "Next One/Aim/Skill Aim Raycast")]
    public class SkillAimRaycast : SkillAimParams
    {
        public float MaxDistance;

        public override void GetTarget(SkillUseParams _useParams)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(_useParams.Origin.transform.position, _useParams.Origin.transform.forward,
                out raycastHit, MaxDistance,
                LayerMask))
            {
                var targetable = raycastHit.collider.GetComponent<Targetable>();
                if (targetable)
                {
                    _useParams.Target = new Vector3Target(_useParams.Origin, targetable.transform);
                }
            }
        }
    }
}