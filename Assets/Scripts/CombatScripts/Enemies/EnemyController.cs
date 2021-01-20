using Assets.Scripts.Utility;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Enemies
{
    public class EnemyController : MonoBehaviour, IDamageable
    {
        //TODO: Nav Mesh Agent

        public EnemyData EnemyData;

        private void Start()
        {
            //Nav Agent Set Up

            if (EnemyData != null)
            {
                LoadEnemy(EnemyData);
            }
        }

        private void LoadEnemy(EnemyData _enemyData)
        {
            //Remove Children object i.e. visuals
            foreach (Transform child in transform)
            {
                if (Application.isEditor)
                {
                    DestroyImmediate(child.gameObject);
                }
                else
                {
                    Destroy(child.gameObject);
                }
            }

            //Load & Configure Enemy Model 
            GameObject model = Instantiate(_enemyData.EnemyModel);
            model.transform.SetParent(transform);
            model.transform.localPosition = Vector3.zero;
            model.transform.rotation = Quaternion.identity;

            //Set Enemy Stats

            //Set Nav Agent Speed
        }

        private void Update()
        {
        }

        public void TakeDamage(int _damage)
        {
            throw new System.NotImplementedException();
        }
    }
}