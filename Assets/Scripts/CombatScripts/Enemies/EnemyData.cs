using UnityEngine;

namespace Assets.Scripts.CombatScripts.Enemies
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
        [SerializeField] private int EnemyDamage = 1;
        [SerializeField] private float EDetectRange = 10f;


        public int Id => EId;

        public string Name => EnemyName;

        public string Description => EnemyDescription;

        public GameObject Model => EnemyModel;

        public int Health => EnemyHealth;

        public float Velocity => EnemyVelocity;

        public int Damage => EnemyDamage;

        public float DetectRange => EDetectRange;
    }
}