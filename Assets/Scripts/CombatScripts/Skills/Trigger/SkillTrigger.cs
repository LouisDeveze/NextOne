using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills.Trigger
{
    public abstract class SkillTrigger : ScriptableObject
    {
        public abstract bool IsTriggered();
    }
}
