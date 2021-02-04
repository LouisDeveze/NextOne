using UnityEngine;

namespace NextOne
{
    [CreateAssetMenu(fileName = "NewSkillAimOverlap", menuName = "Next One/Aim/Skill Aim Overlap")]
    public class SkillAimOverlap : SkillAimParams
    {
        public override void GetTarget(SkillUseParams _useParams)
        {
            PlayerTarget playerTarget = new PlayerTarget(_useParams.Origin);
            var colliders = Physics.OverlapSphere(_useParams.Origin.transform.position, _useParams.Radius, LayerMask);
            foreach (var collider in colliders)
            {
                var targetable = collider.GetComponent<Targetable>();

                if (!targetable) continue;

                var playerHit = targetable.GetComponent<PlayerController>();
                if (playerHit)
                    playerTarget.AddTarget(playerHit);
            }

            _useParams.Target = playerTarget;
        }
    }
}