using UnityEngine;

namespace Assets.Scripts.CombatScripts.Enemies
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Next One/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        //TODO: Make it unique
        public int Id = 0;
        public string EnemyName = "default";
        public string EnemyDescription = "default description";
        public GameObject EnemyModel;
        
        public int EnemyHealth = 1;
        public float EnemySpeed = 1f;
        public int EnemyDamage = 1;
        public float DetectRange = 10f;
    }
}
