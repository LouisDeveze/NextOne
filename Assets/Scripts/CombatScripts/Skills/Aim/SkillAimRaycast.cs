using UnityEngine;

namespace NextOne
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
                Debug.Log(raycastHit.transform.name);
                _useParams.Target = new Vector3Target(_useParams.Origin, raycastHit.transform);
            }
        }
    }
}