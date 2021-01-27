using UnityEditor;
using UnityEngine;

namespace NextOne
{
    [CreateAssetMenu(fileName = "Skill Trigger Key", menuName = "Next One/Skills/Skill Trigger Key")]
    public class SkillTriggerKey : SkillTrigger
    {
        public KeyCode KeyCode;

        public override bool IsTriggered()
        {
            return Input.GetKeyDown(KeyCode);
        }
    }
}