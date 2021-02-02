using UnityEditor;
using UnityEngine;

namespace NextOne
{
    [CreateAssetMenu(fileName = "Skill Trigger Key", menuName = "Next One/Skills/Triggers/Skill Trigger Key")]
    public class SkillTriggerKey : SkillTrigger
    {
        public KeyCode KeyCode;

        public override bool IsTriggered(SkillUseParams _useParams)
        {
            return Input.GetKeyDown(KeyCode);
        }
    }
}