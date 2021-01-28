using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NextOne
{
    public class EnemyController : MonoBehaviour, IDamageable
    {
        //TODO: Nav Mesh Agent

        [SerializeField] private EnemyData EnemyData = null;

        //Game Context
        private GameContext ctx;

        //Enemy Stats
        private int EnemyHealthPoint;
        private float EnemyVelocity;
        private float EnemyAngularVelocity;

        //Enemy Skills
        //Skills Script References
        private List<ISkill> Skills = new List<ISkill>();

        //Enemy controls
        private GameObject EnemyModel;
        private Rigidbody EnemyRigidbody;
        private CapsuleCollider EnemyCollider;
        private Animator EnemyAnimator;
        AIEntityControl AiEntityControl = null;
        private bool EnemyCanMove = true;
        public bool SkillInUse = false;

        //ACTIONS
        private List<ScriptableAction> OnDeathActions = new List<ScriptableAction>();

        private void Start()
        {
            //TODO: Louis check
            this.ctx = GameObject.Find("State Manager").GetComponent<GameContext>();

            //Nav Agent Set Up

            if (EnemyData != null)
                LoadEnemy(EnemyData);
        }

        private void LoadEnemy(EnemyData _enemyData)
        {
            //Load & Configure Enemy Model 
            EnemyModel = Instantiate(_enemyData.Model, transform);
            EnemyRigidbody = EnemyModel.GetComponent<Rigidbody>();
            EnemyCollider = EnemyModel.GetComponent<CapsuleCollider>();
            EnemyAnimator = EnemyModel.GetComponent<Animator>();

            //Set Enemy Stats
            EnemyAngularVelocity = _enemyData.AngularVelocity;
            EnemyVelocity = _enemyData.Velocity;

            //Set Components
            AttachComponents();

            //Set Skills
            AttachInitialSkills();

            //Set Behavior
            AttachInitialBehavior();

            //Set Enemy Stats
            EnemyHealthPoint = _enemyData.Health;
            EnemyVelocity = _enemyData.Velocity;

            //Get actions
            OnDeathActions = _enemyData.OnDeathActions;

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
            foreach (var skill in EnemyData.SkillsData)
            {
                Skills.Add(skill.AttachComponentTo(gameObject));
            }
        }

        private void DetachSkills()
        {
            foreach (var skill in Skills)
            {
                skill.Detach();
            }

            Skills.Clear();
        }

        private void AttemptSkill(int _index)
        {
            //If not using an other skill
            if (SkillInUse)
                return;

            //If can cast skill
            if (!Skills[_index].CanCast())
                return;
            var skillParams = new SkillUseParams {Origin = EnemyModel};
            SkillInUse = true;
            Skills[_index].Use(skillParams);
        }

        private void Update()
        {
            EnemyMovementUpdate();
            EnemySkillUpdate();
        }

        private void EnemySkillUpdate()
        {
        }

        private void EnemyMovementUpdate()
        {
        }

        void OnActionOverEnemy(List<EnemyController> _enemies)
        {
            // AttackTarget();
        }


        public void TakeDamage(int _damageTaken)
        {
            EnemyHealthPoint = Mathf.Clamp(EnemyHealth - _damageTaken, 0, EnemyData.Health);
            if (EnemyHealth <= 0)
            {
                StartCoroutine(KillEnemy());
            }
        }

        IEnumerator KillEnemy()
        {
            //TODO: Animator | Audio | OnDeathSO

            foreach (var action in OnDeathActions)
            {
                action.PerformAction(this.EnemyModel);
            }

            //SceneManager
            return null;
        }

        public void Heal(int _healthAmount)
        {
            EnemyHealthPoint = Mathf.Clamp(EnemyHealth + _healthAmount, 0, EnemyData.Health);
        }

        public void CanMove(bool _canMove)
        {
            EnemyCanMove = _canMove;
        }

        public void ResetTriggersAnimator()
        {
            Animations.ResetTriggers(EnemyAnimator);
        }

        public bool IsAnimationLastAtLeast(float _animationTime, int layer)
        {
            return (EnemyAnimator.GetCurrentAnimatorStateInfo(layer).normalizedTime >= _animationTime);
        }


        public bool HasAnimatorPlaying(EAnimation animation, int layer)
        {
            return EnemyAnimator.GetCurrentAnimatorStateInfo(layer).IsName(Animations.GetStringEquivalent(animation));
        }

        public void SetTriggerAnimator(EAnimation _animationName)
        {
            EnemyAnimator.SetTrigger(Animations.GetStringEquivalent(_animationName));
        }

        //SETTER AND GETTER

        public List<Transform> GetCastPoint(ECastPoint _eCastPoint)
        {
            switch (_eCastPoint)
            {
                case ECastPoint.Enemy:
                    return GetEnemyCastPoint();
            }

            return new List<Transform>();
        }

        private List<Transform> GetEnemyCastPoint()
        {
            return new List<Transform>();
        }

        public int EnemyHealth => EnemyHealthPoint;

        public EnemyData Data => EnemyData;
    }
}