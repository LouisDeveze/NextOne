using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using NextOne.Controllers;
using UnityEngine;
using UnityEngine.AI;

namespace NextOne
{
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

        Vector3 CurrentFacing = Vector3.zero;
        Vector3 LastFacing = Vector3.zero;

        //Activate status
        private bool Activated = false;

        //ACTIONS
        private List<ScriptableAction> OnDeathActions = new List<ScriptableAction>();


        private void Start()
        {
            this.ctx = GameObject.Find("State Manager").GetComponent<GameContext>();

            Player = ctx.playerController;

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

            EntityAgent = EnemyModel.GetComponent<NavMeshAgent>();
            EntityAgent.updateRotation = true;
            EntityAgent.updatePosition = true;

            //Set Enemy Stats
            // EnemyAngularVelocity = _enemyData.AngularVelocity;
            EntityAgent.angularSpeed = _enemyData.AngularVelocity;
            //  EnemyVelocity = _enemyData.Velocity;
            EntityAgent.speed = _enemyData.Velocity;
            EntityAgent.radius = _enemyData.AvoidanceRange;
            EntityAgent.stoppingDistance = _enemyData.StoppingDistance;

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
            float distanceToPlayer = Vector3.Distance(Player.Model.transform.position, EnemyModel.transform.position);

            Activated = distanceToPlayer <= EnemyData.DetectRange;

            ActivateMecha();

            //ToTarget = Activated ? Player.Model.transform : transform;
            ToTarget = Player.Model.transform;

            if (!Activated)
                return;

            CurrentFacing = EnemyModel.transform.forward;

            EnemyMovementUpdate();
            EntityAnimationUpdate();
            EnemySkillUpdate();
        }

        private void EnemySkillUpdate()
        {
            float distanceToPlayer = Vector3.Distance(Player.Model.transform.position, EnemyModel.transform.position);
            var list = EnemyData.SkillsData;
            
            SkillUseParams useParams = new SkillUseParams
            {
                DistanceToPlayer = distanceToPlayer,
                DetectRange = EnemyData.AttackRange
            };

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

            Agent.velocity = Agent.remainingDistance > Agent.stoppingDistance ? Agent.desiredVelocity : Vector3.zero;
        }


        void OnActionOverEnemy(List<EnemyController> _enemies)
        {
            // AttackTarget();
        }


        public override void TakeDamage(int _damageTaken)
        {
            EnemyHealthPoint = Mathf.Clamp(EnemyHealth - _damageTaken, 0, EnemyData.Health);
            Debug.Log("Current Health point:" + EnemyHealth);
            if (EnemyHealth <= 0)
            {
                KillEnemy();
            }
        }

        void KillEnemy()
        {
            //TODO: Animator | Audio | OnDeathSO

            foreach (var action in OnDeathActions)
            {
                action.PerformAction(this.EnemyModel);
            }

            Destroy(this.gameObject);
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
            if (!EnemyCanMove)
                return;

            float currentAngularVelocity =
                Vector3.Angle(CurrentFacing, LastFacing) / Time.deltaTime; //degrees per second

            LastFacing = CurrentFacing;

            float velocity = EntityAgent.velocity.magnitude / EntityAgent.speed;
            AnimateMovement(EnemyAnimator, Vector3.forward * velocity, currentAngularVelocity);
        }

        private void ActivateMecha()
        {
            if (Activated)
            {
                Animations.ResetTriggers(EnemyAnimator);
                EnemyAnimator.SetTrigger(Animations.GetStringEquivalent(EAnimation.GettingUp));
                //Debug.Log("Mecha Activated: " + this.GetInstanceID());
            }
            else
            {
                Animations.ResetTriggers(EnemyAnimator);
                EnemyAnimator.SetTrigger(Animations.GetStringEquivalent(EAnimation.Idle));
            }
        }

        private void AnimateMovement(Animator _entityAnimator, Vector3 _movement, float _angle)
        {
            string trigger;

            // Else check the movement in Z to now if player is running backward or frontward
            if (_movement.z > 0)
            {
                trigger = Animations.GetStringEquivalent(EAnimation.RunFront);
            }
            else if (_movement.magnitude == 0 && _angle < -1)
            {
                trigger = Animations.GetStringEquivalent(EAnimation.TurnRight);
            }
            // Else if idle and turning a lot
            else if (_movement.magnitude == 0 && _angle > 1)
            {
                trigger = Animations.GetStringEquivalent(EAnimation.TurnLeft);
            }
            // Idle triggered when there are no movement
            else

            {
                if (!Activated) return;
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
            List<Transform> castPoints = new List<Transform>
            {
                EnemyModel.GetComponentInChildren<CastPoint>().transform
            };
            Debug.Log("LIST CAST POINT ENEMY" + GetInstanceID());
            return castPoints;
        }

        void OnDrawGizmos()
        {
            // Draw attack sphere 
            Gizmos.color = new Color(255f, 0, 0, .5f);
            Gizmos.DrawWireSphere(EnemyModel.transform.position, EnemyData.AttackRange);

            // Draw chase sphere 
            Gizmos.color = new Color(0, 0, 255, .5f);
            Gizmos.DrawWireSphere(EnemyModel.transform.position, EnemyData.DetectRange);
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