using System.Collections.Generic;
using Assets.Scripts.CombatScripts.Enemies;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills.Aim
{
    public abstract class SkillAimParams : SkillAim
    {
        public delegate void OnActionOverEnemy(List<EnemyController> _enemyControllers);

        public event OnActionOverEnemy onActionOverEnemy;
        public LayerMask LayerMask;
    }
}