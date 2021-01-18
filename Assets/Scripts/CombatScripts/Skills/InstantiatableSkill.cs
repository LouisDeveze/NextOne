using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills
{
    public class InstantiatableSkill : Skill
    {
        public GameObject PrefabToSpawn;
        public float Velocity;

        public override void Use(Transform _origin, Target _target)
        {
            var instance =
                Instantiate(PrefabToSpawn, _origin.position + _origin.forward, _origin.rotation) as GameObject;
            var rb = instance.GetComponent<Rigidbody>();
            if (rb != null) rb.AddForce(_origin.forward * Velocity);
        }

        [MenuItem("Assets/Create/Skills/Instantiatable Skill")]
        public static void CreateInstantiatableSkillAsset()
        {
            InstantiatableSkill asset = ScriptableObject.CreateInstance<InstantiatableSkill>();
            AssetDatabase.CreateAsset(asset, "Assets/NewInstantiatableSkillObject.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}