using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills.Aim
{
    public abstract class SkillAim : ScriptableObject
    {
        public abstract Target GetTarget(Transform _origin);
    }
}
