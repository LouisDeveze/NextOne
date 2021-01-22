using UnityEngine;

namespace NextOne
{
    [CreateAssetMenu(fileName = "NewSkillAimOverlapEnemy", menuName = "Next One/Aim/Skill Aim Overlap Enemy")]
    public class SkillAimRayCastEnemy : SkillAimParams
    {
        public float MaxDistance;
        public float radius;

        public override void GetTarget(SkillUseParams _useParams)
        {
            EnemyTarget enemyTarget = new EnemyTarget(_useParams.Origin);
            var colliders = Physics.OverlapSphere(_useParams.Origin.transform.position, radius, LayerMask);
            foreach (var collider in colliders)
            {
                var targetable = collider.GetComponent<Targetable>();
                if (targetable)
                {
                    var enemyHit = targetable.GetComponent<EnemyController>();
                    if (enemyHit)
                        enemyTarget.AddTarget(enemyHit);
                }
            }

            _useParams.Target = enemyTarget;
        }
    }
}