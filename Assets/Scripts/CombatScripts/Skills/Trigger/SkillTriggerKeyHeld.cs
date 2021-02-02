using UnityEditor;
using UnityEngine;

namespace NextOne
{
    [CreateAssetMenu(fileName = "Skill Trigger Key Held", menuName = "Next One/Skills/Triggers/Skill Trigger Key Held")]
    public class SkillTriggerKeyHeld : SkillTriggerKey
    {
        public override bool IsTriggered(SkillUseParams _useParams)
        {
            return Input.GetKey(KeyCode);
        }
    }
}