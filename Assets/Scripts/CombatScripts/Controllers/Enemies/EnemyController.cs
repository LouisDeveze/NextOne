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

        private float AngleToPlayer = 0f;
        private float velocityMagnitude = 0f;

        private bool once = false;

        //Activate status
        private bool Activated = false;

        //ACTIONS
        private List<ScriptableAction> OnDeathActions = new List<ScriptableAction>();

        private ClawCollider ClawColliderRight;
        private ClawCollider ClawColliderLeft;

        //TODO: Active Trigger

        public void ActiveClawsTrigger(bool _active)
        {
            ClawColliderRight.ActiveTrigger(_active);
            ClawColliderLeft.ActiveTrigger(_active);
        }

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

            //Set Behavior
            AttachInitialBehavior();

            //Set Skills
            AttachInitialSkills();


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
            //Attach Claws

            ClawCollider[] colliders = EnemyModel.GetComponentsInChildren<ClawCollider>();

            ClawColliderRight = colliders[0];
            ClawColliderLeft = colliders[1];

            ClawColliderLeft.Damage = ((MeleeSkillData) EnemyData.SkillsData[0]).Damage;

            ClawColliderRight.Damage = ((MeleeSkillData) EnemyData.SkillsData[0]).Damage;
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

            if (Player.Health <= 0) return;
            for (var index = 0; index < list.Count; index++)
            {
                var skill = list[index];
                if (skill.Trigger.IsTriggered(useParams))
                    AttemptSkill(index);
            }
        }

        private void EnemyMovementUpdate()
        {
            velocityMagnitude = EntityAgent.velocity.magnitude / EntityAgent.speed;

            if (velocityMagnitude == 0)
            {
                Vector3 toPlayer = this.Player.Model.transform.position - this.EnemyModel.transform.position;
                AngleToPlayer = Vector3.SignedAngle(toPlayer, this.EnemyModel.transform.forward, Vector3.up);
            }


            // Calculate Variation Angle
            float variationAngle = AngleToPlayer > 0
                ? Time.deltaTime * Agent.angularSpeed
                : -Time.deltaTime * Agent.angularSpeed;
            variationAngle = Mathf.Abs(variationAngle) > Mathf.Abs(AngleToPlayer) ? AngleToPlayer : variationAngle;
            variationAngle = Mathf.Abs(AngleToPlayer) > 10 ? variationAngle : 0;

            // If the skill attack is in use, we don't won't our friend to rotate so
            if (SkillInUse)
            {
                Agent.enabled = false;
                Agent.updateRotation = false;
                Agent.updatePosition = false;
                AngleToPlayer = 0;
                variationAngle = 0;
            }
            else
            {
                Agent.enabled = true;
                Agent.updateRotation = true;
                Agent.updatePosition = true;
            }

            // Finally Rotate the model
            if (velocityMagnitude == 0)
                EnemyModel.transform.Rotate(Vector3.up, -variationAngle);

            if (!ToTarget)
                return;

            if (Agent.enabled)
                Agent.SetDestination(ToTarget.position);
            if (Agent.enabled)
                Agent.velocity = Agent.remainingDistance > Agent.stoppingDistance
                    ? Agent.desiredVelocity
                    : Vector3.zero;
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


            AnimateMovement(EnemyAnimator, Vector3.forward * velocityMagnitude, AngleToPlayer);
        }

        private void ActivateMecha()
        {
            if (!Activated || once) return;

            Animations.ResetTriggers(EnemyAnimator);
            EnemyAnimator.SetTrigger(Animations.GetStringEquivalent(EAnimation.GettingUp));
            //Debug.Log("Mecha Activated: " + this.GetInstanceID());
            once = true;
        }

        private void AnimateMovement(Animator _entityAnimator, Vector3 _movement, float _angle)
        {
            string trigger;

            // Else check the movement in Z to now if player is running backward or frontward
            if (_movement.z > 0)
            {
                trigger = Animations.GetStringEquivalent(EAnimation.RunFront);
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