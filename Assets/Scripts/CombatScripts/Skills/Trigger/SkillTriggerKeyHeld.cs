using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills.Trigger
{
    [CreateAssetMenu(fileName = "Skill Trigger Key Held", menuName = "Next One/Skills/Skill Trigger Key Held")]
    public class SkillTriggerKeyHeld : SkillTriggerKey
    {
        public override bool IsTriggered()
        {
            return Input.GetKey(KeyCode);
        }
    }
}