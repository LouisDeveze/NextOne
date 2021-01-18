using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills.Trigger
{
    public class SkillTriggerKey : SkillTrigger
    {
        public KeyCode KeyCode;

        public override bool IsTriggered()
        {
            return Input.GetKeyDown(KeyCode);
        }

        [MenuItem("Assets/Create/Skills/Trigger/Skill Trigger Key")]
        public static void CreateSkillTriggerKeyAsset()
        {
            SkillTriggerKey asset = ScriptableObject.CreateInstance<SkillTriggerKey>();
            AssetDatabase.CreateAsset(asset, "Assets/NewSkillTriggerKeyObject.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}