using Assets.Scripts.CombatScripts.Enemies;
using Assets.Scripts.Utility;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills.Aim
{
    [CreateAssetMenu(fileName = "NewSkillAimRaycastEnemy", menuName = "Next One/Aim/Skill Aim Raycast Enemy")]
    public class SkillAimRaycastEnemy : SkillAimParams
    {
        public float MaxDistance;

        public override void GetTarget(SkillUseParams _skillUseParams)
        {
            if (!_skillUseParams.Origin)
                Debug.Log("no origin");

            RaycastHit raycastHit;
            if (!Physics.Raycast(_skillUseParams.Origin.transform.position, _skillUseParams.Origin.transform.forward,
                out raycastHit, MaxDistance,
                LayerMask)) return;


            var targetable = raycastHit.collider.transform.parent.GetComponent<Targetable>();
            Debug.Log(targetable.gameObject.name);

            if (!targetable) return;

            Debug.Log("Here");

            var enemyHit = targetable.GetComponent<EnemyController>();
            if (enemyHit)
            {
                _skillUseParams.Target = new EnemyTarget(_skillUseParams.Origin, enemyHit);
            }
        }
    }
}