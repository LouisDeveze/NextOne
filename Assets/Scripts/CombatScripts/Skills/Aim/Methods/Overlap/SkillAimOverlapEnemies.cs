using UnityEngine;

namespace NextOne
{
    [CreateAssetMenu(fileName = "NewSkillAimOverlapEnemy", menuName = "Next One/Aim/Skill Aim Overlap Enemy")]
    public class SkillAimOverlapEnemies : SkillAimParams
    {
        public override void GetTarget(SkillUseParams _useParams)
        {
            EnemyTarget enemyTarget = new EnemyTarget(_useParams.Origin);
            var colliders = Physics.OverlapSphere(_useParams.Origin.transform.position, _useParams.Radius, LayerMask);
            foreach (var collider in colliders)
            {
                var targetable = collider.GetComponent<Targetable>();

                if (!targetable) continue;

                var enemyHit = targetable.GetComponent<EnemyController>();
                if (enemyHit)
                    enemyTarget.AddTarget(enemyHit);
            }

            _useParams.Target = enemyTarget;
        }
    }
}