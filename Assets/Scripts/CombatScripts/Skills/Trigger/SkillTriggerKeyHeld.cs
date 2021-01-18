using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills.Trigger
{
    public class SkillTriggerKeyHeld : SkillTriggerKey
    {
        public override bool IsTriggered()
        {
            return Input.GetKey(KeyCode);
        }

        [MenuItem("Assets/Create/Skills/Trigger/Skill Trigger Key Held")]
        public static void CreateSkillTriggerKeyHeldAsset()
        {
            SkillTriggerKeyHeld asset = ScriptableObject.CreateInstance<SkillTriggerKeyHeld>();
            AssetDatabase.CreateAsset(asset, "Assets/NewSkillTriggerKeyHeldObject.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}