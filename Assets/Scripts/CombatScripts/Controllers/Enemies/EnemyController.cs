using System;
using System.Collections;
using System.Collections.Generic;
using NextOne.Controllers;
using UnityEngine;
using UnityEngine.AI;

namespace NextOne
{
    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof(PlayerController))]
    public class EnemyController : EntityController
    {
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
        private NavMeshAgent EntityAgent;
        private Transform EntityToTarget;

        private PlayerController Player;

        private Animator EnemyAnimator;
        private bool EnemyCanMove = true;

        //MOVEMENT
        // Movement in World Space
        private Vector3 Movement;

        //Movement in Model Space
        private Vector3 ModelMovement;

        //ACTIONS
        private List<ScriptableAction> OnDeathActions = new List<ScriptableAction>();
        private float Angle;
        private float thresholdStrafe = .5f;


        private void Start()
        {
            this.ctx = GameObject.Find("State Manager").GetComponent<GameContext>();

            EntityAgent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            Player = GetComponent<PlayerController>();

            EntityAgent.updateRotation = false;
            EntityAgent.updatePosition = true;

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
            float distanceToPlayer = Vector3.Distance(Player.transform.position, transform.position);

            ToTarget = distanceToPlayer <= EnemyData.DetectRange ? Player.transform : transform;

            EnemyMovementUpdate();
            EntityAnimationUpdate();
            EnemySkillUpdate();
        }

        private void EnemySkillUpdate()
        {
            float distanceToPlayer = Vector3.Distance(Player.transform.position, transform.position);

            var list = EnemyData.SkillsData;
            SkillUseParams useParams = new SkillUseParams {DistanceToPlayer = distanceToPlayer};

            for (var index = 0; index < list.Count; index++)
            {
                var skill = list[index];
                if (skill.Trigger.IsTriggered(useParams))
                    AttemptSkill(index);
            }
        }

        private void EnemyMovementUpdate()
        {
            if (!ToTarget)
                return;

            Agent.SetDestination(ToTarget.position);

            if (Agent.remainingDistance > Agent.stoppingDistance)
            {
                Movement = Agent.desiredVelocity;
                Move(Movement);
            }
            else
            {
                if (GetComponent<EnemyController>())
                {
                    Agent.velocity = Vector3.zero;
                }

                Movement = Vector3.zero;
                Move(Vector3.zero);
            }
        }

        private void Move(Vector3 _move)
        {
            if (_move.magnitude > 1f) _move.Normalize();
            _move = transform.InverseTransformDirection(_move);

            Movement = _move * EnemyVelocity;
            EnemyRigidbody.velocity = Movement;
        }


        void OnActionOverEnemy(List<EnemyController> _enemies)
        {
            // AttackTarget();
        }


        public override void TakeDamage(int _damageTaken)
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

        public override void CanMove(bool _canMove)
        {
            EnemyCanMove = _canMove;
        }

        public override void ResetTriggersAnimator()
        {
            Animations.ResetTriggers(EnemyAnimator);
        }

        public override bool IsAnimationLastAtLeast(float _animationTime, int layer)
        {
            return (EnemyAnimator.GetCurrentAnimatorStateInfo(layer).normalizedTime >= _animationTime);
        }


        public override bool HasAnimatorPlaying(EAnimation animation, int layer)
        {
            return EnemyAnimator.GetCurrentAnimatorStateInfo(layer).IsName(Animations.GetStringEquivalent(animation));
        }

        public override void SetTriggerAnimator(EAnimation _animationName)
        {
            EnemyAnimator.SetTrigger(Animations.GetStringEquivalent(_animationName));
        }

        private void EntityAnimationUpdate()
        {
            ModelMovement = EnemyModel.transform.InverseTransformDirection(Movement);
            if (EnemyCanMove)
                AnimateMovement(EnemyAnimator, ModelMovement, Angle);
        }

        private void AnimateMovement(Animator _entityAnimator, Vector3 _movement, float _angle)
        {
            string trigger = Animations.GetStringEquivalent(EAnimation.Idle);


            // If movement in right is superior to the Strafe threshold, Strafe Right
            if (_movement.x > thresholdStrafe)
            {
                trigger = Animations.GetStringEquivalent(EAnimation.StrafeRight);
            }
            // If movement in Left is superior to the Strafe threshold, Strafe Left
            else if (_movement.x < -thresholdStrafe)
            {
                trigger = Animations.GetStringEquivalent(EAnimation.StrafeLeft);
            }
            // Else check the movement in Z to now if player is running backward or frontward
            else if (_movement.z > 0)
            {
                trigger = Animations.GetStringEquivalent(EAnimation.RunFront);
            }
            // Else check the movement in Z to now if player is running backward or frontward
            else if (_movement.z < 0)
            {
                trigger = Animations.GetStringEquivalent(EAnimation.RunBack);
            }
            else if (_movement.magnitude == 0 && _angle < -10)
            {
                trigger = Animations.GetStringEquivalent(EAnimation.TurnRight);
            }
            // Else if idle and turning a lot
            else if (_movement.magnitude == 0 && _angle > 10)
            {
                trigger = Animations.GetStringEquivalent(EAnimation.TurnLeft);
            }
            // Idle triggered when there are no movement
            else
            {
                trigger = Animations.GetStringEquivalent(EAnimation.Idle);
            }

            Animations.ResetTriggers(_entityAnimator);
            _entityAnimator.SetTrigger(trigger);
        }

        //SETTER AND GETTER

        public override List<Transform> GetCastPoint(ECastPoint _eCastPoint)
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
            //TODO: implement function
            return new List<Transform>();
        }

        void OnDrawGizmos()
        {
            // Draw attack sphere 
            Gizmos.color = new Color(255f, 0, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, EnemyData.AttackRange);

            // Draw chase sphere 
            Gizmos.color = new Color(0, 0, 255, .5f);
            Gizmos.DrawWireSphere(transform.position, EnemyData.DetectRange);
        }

        public int EnemyHealth => EnemyHealthPoint;

        public EnemyData Data => EnemyData;

        public NavMeshAgent Agent
        {
            get => EntityAgent;
            set => EntityAgent = value;
        }

        public Transform ToTarget
        {
            get => EntityToTarget;
            set => EntityToTarget = value;
        }
    }
}