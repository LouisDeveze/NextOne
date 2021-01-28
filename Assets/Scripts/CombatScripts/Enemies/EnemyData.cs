using System.Collections.Generic;
using UnityEngine;

namespace NextOne
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Next One/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        //TODO: Make it unique
        [SerializeField] private int EId = 0;
        [SerializeField] private string EnemyName = "default";
        [SerializeField] private string EnemyDescription = "default description";
        [SerializeField] private GameObject EnemyModel;

        [SerializeField] private int EnemyHealth = 1;
        [SerializeField] private float EnemyVelocity = 1f;
        [SerializeField] private float EnemyAngularVelocity = 1f;
        [SerializeField] private float EDetectRange = 10f;

        [SerializeField] private List<ScriptableAction> OnEnemyDeathActions = new List<ScriptableAction>();
        [SerializeField] private List<SkillData> EnemySkillsData = new List<SkillData>();

        public int Id => EId;

        public string Name => EnemyName;

        public string Description => EnemyDescription;

        public GameObject Model => EnemyModel;

        public int Health => EnemyHealth;

        public float Velocity => EnemyVelocity;
        public float AngularVelocity => EnemyAngularVelocity;

        public float DetectRange => EDetectRange;

        public List<ScriptableAction> OnDeathActions => OnEnemyDeathActions;

        public List<SkillData> SkillsData => EnemySkillsData;
    }
}