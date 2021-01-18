using Assets.Script.Utility;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills.Aim
{
    public class SkillAimRaycast : SkillAimPhysics
    {
        public float MaxDistance;

        public override Target GetTarget(Transform _origin)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(_origin.position, _origin.forward, out raycastHit, MaxDistance, LayerMask))
            {
                var targetable = raycastHit.collider.GetComponent<Targetable>();
                if (targetable != null) return new Vector3Target(targetable.transform);
            }

            return null;
        }

        [MenuItem("Assets/Create/Skills/Aim/Skill Aim Raycast")]
        public static void CreateSkillAimRaycastAsset()
        {
            SkillAimRaycast asset = ScriptableObject.CreateInstance<SkillAimRaycast>();
            AssetDatabase.CreateAsset(asset, "Assets/NewSkillAimRaycastObject.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}