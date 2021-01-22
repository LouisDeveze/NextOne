using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NextOne
{
    public class EnemyController : MonoBehaviour, IDamageable
    {
        //TODO: Nav Mesh Agent

        public EnemyData EnemyData;

        //Enemy Stats
        private int HealthPoint;
        private float Velocity;

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
            //Load & Configure Enemy Model 
            GameObject model = Instantiate(_enemyData.Model);
            model.transform.SetParent(transform);
            model.transform.localPosition = Vector3.zero;
            model.transform.rotation = Quaternion.identity;

            //Set Components
            AttachComponents();

            //Set Skills
            AttachInitialSkills();

            //Set Behavior
            AttachInitialBehavior();

            //Set Enemy Stats
            HealthPoint = _enemyData.Health;
            Velocity = _enemyData.Velocity;

            //Set Nav Agent Speed
        }

        //COMPONENT RELATED
        private void AttachComponents()
        {
            this.gameObject.AddComponent<Targetable>();
        }


        private void AttachInitialBehavior()
        {
        }

        private void AttachInitialSkills()
        {
        }

        private void AttemptSkill(int _index)
        {
        }

        private void Update()
        {
        }

        void OnActionOverEnemy(List<EnemyController> _enemies)
        {
            // AttackTarget();
        }


        public void TakeDamage(int _damageTaken)
        {
            HealthPoint = Mathf.Clamp(HealthPoint - _damageTaken, 0, EnemyData.Health);
            if (HealthPoint <= 0)
            {
                StartCoroutine(KillEnemy());
            }
        }

        IEnumerator KillEnemy()
        {
            //TODO: Animator & Audio
            //SceneManager
            return null;
        }

        public void Heal(int _healthAmount)
        {
            HealthPoint = Mathf.Clamp(HealthPoint + _healthAmount, 0, EnemyData.Health);
        }

        public int EnemyHealth => HealthPoint;
    }
}