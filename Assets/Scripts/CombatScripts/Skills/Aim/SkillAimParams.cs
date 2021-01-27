using System.Collections.Generic;
using UnityEngine;

namespace NextOne
{
    public abstract class SkillAimParams : SkillAim
    {
        public delegate void OnActionOverEnemy(List<EnemyController> _enemyControllers);

        public event OnActionOverEnemy onActionOverEnemy;
        public LayerMask LayerMask;
    }
}