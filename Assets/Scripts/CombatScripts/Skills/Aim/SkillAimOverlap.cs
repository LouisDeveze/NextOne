using Assets.Script.Utility;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills.Aim
{
    public class SkillAimOverlap : SkillAimPhysics
    {
        public float radius;

        public override Target GetTarget(Transform _origin)
        {
            var colliders = Physics.OverlapSphere(_origin.position, radius);
            foreach (var collider in colliders)
            {
                var targetable = collider.GetComponent<Targetable>();
                if (targetable != null) return new Vector3Target(targetable.transform);
            }

            return null;
        }

        [MenuItem("Assets/Create/Skills/Aim/Skill Aim Overlap")]
        public static void CreateSkillAimOverlapAsset()
        {
            SkillAimOverlap asset = ScriptableObject.CreateInstance<SkillAimOverlap>();
            AssetDatabase.CreateAsset(asset, "Assets/NewSkillAimOverlapObject.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}