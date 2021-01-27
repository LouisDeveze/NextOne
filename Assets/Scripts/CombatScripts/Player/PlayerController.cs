using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NextOne
{
    public class PlayerController : MonoBehaviour, IDamageable
    {
        //TODO: Game Logic

        [SerializeField] private PlayerData PlayerData = null;
        [SerializeField] private Transform CastAnchorPoint = null;

        //Game Context
        private GameContext ctx;

        //Player Stats
        private int HealthPoint;
        private float Velocity;
        private float AngularVelocity;
        private bool PlayerCanMove = true;
        public bool SkillInUse = false;

        //Weapon Stats
        private List<WeaponController> Weapons = new List<WeaponController>();
        private int CurrentWeapon = 0;
        private bool IsCasting = false;

        //Skills Script References
        private List<ISkill> Skills = new List<ISkill>();

        private GameObject PlayerModel;
        private Rigidbody PlayerRigidbody;
        private CapsuleCollider PlayerCollider;
        private Animator PlayerAnimator;

        //UTILS
        private float distanceFromCamera = 0;
        private Plane plane = new Plane(Vector3.up, new Vector3(0, 0, 0));
        private float angularTreshold = 5;
        private float angle;

        //MOVEMENT
        // Movement in World Space
        private Vector3 movement;

        //Movement in Model Space
        private Vector3 modelMovement;

        private const float movementSmoother = .4f;

        private float prevMagnitude = 0;
        // public bool UsingSkill;

        // UI
        private SkillUI PlayerAutoAttackUI;
        private SkillUI PlayerPrimaryUI;
        private SkillUI PlayerSecondaryUI;
        private SkillUI PlayerTertiaryUI;
        

        private void Start()
        {
            this.ctx = GameObject.Find("State Manager").GetComponent<GameContext>();

            if (PlayerData != null)
                LoadPlayer(PlayerData);
        }

        private void LoadPlayer(PlayerData _playerData)
        {
            //Load & Configure Player Model 
            PlayerModel = Instantiate(_playerData.Model, transform);

            PlayerRigidbody = PlayerModel.GetComponent<Rigidbody>();
            PlayerCollider = PlayerModel.GetComponent<CapsuleCollider>();
            PlayerAnimator = PlayerModel.GetComponent<Animator>();

            //Set Player Stats
            AngularVelocity = _playerData.AngularVelocity;
            Velocity = _playerData.Velocity;

            //Attach Component
            AttachComponents();

            //Set Weapon Stats
            CurrentWeapon = _playerData.WeaponHolder.DefaultWeapon;

            // Get Skill UI and Initialize them
            PlayerAutoAttackUI = this.ctx.SkillAutoAttackUI;
            PlayerPrimaryUI = this.ctx.SkillPrimaryUI;
            PlayerSecondaryUI = this.ctx.SkillSecondaryUI;
            PlayerTertiaryUI = this.ctx.SkillTertiaryUI;


            WeaponAnchors[] weaponAnchorsArray = GetComponentsInChildren<WeaponAnchors>();

            foreach (var baseWeaponData in _playerData.WeaponHolder.WeaponsData)
            {
                
                Weapons.Add(baseWeaponData.InstantiateWeapon(new List<WeaponAnchors>(weaponAnchorsArray)));
            }

            //Set Weapon & Skill
            ActivateCurrentWeapon();

            HealthPoint = _playerData.Health;
            Velocity = _playerData.Velocity;
        }


        //COMPONENT RELATED
        private void AttachComponents()
        {
            gameObject.AddComponent<Targetable>();
        }


        //SKILL RELATED
        private void AttachSkills()
        {
            foreach (var skill in PlayerData.WeaponHolder.GetWeaponDataAt(CurrentWeapon).Skills)
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


        //Check if all unique
        private bool CheckSkillTrigger()
        {
            //
            return false;
        }

        private void AttemptSkill(int _index)
        {
            if (SkillInUse)
                return;


            //TODO: check cooldown

            //IF CASTING

            var skillParams = new SkillUseParams {Origin = PlayerModel};
            SkillInUse = true;
            Skills[_index].Use(skillParams);
        }

        //WEAPON RELATED

        private void ActivateCurrentWeapon()
        {
            Weapons[CurrentWeapon].SetActive(true);
            PlayerAnimator.runtimeAnimatorController = Weapons[CurrentWeapon].WeaponAnimatorOverride;
            AttachSkills();
        }

        private void DeactivateUnfocusedWeapon()
        {
            for (int i = 0; i < Weapons.Count; i++)
            {
                if (i == CurrentWeapon)
                    return;

                Weapons[i].SetActive(false);
            }
        }

        private void DeactivateCurrentWeapon()
        {
            Weapons[CurrentWeapon].SetActive(false);
            DetachSkills();
        }

        private void SafeIndent()
        {
            if (CurrentWeapon + 1 < Weapons.Count)
                CurrentWeapon += 1;
            else CurrentWeapon = 0;
        }

        public bool AttemptWeaponChange()
        {
            return Weapons.Count >= 2;
        }

        public void ChangeWeapon()
        {
            DeactivateCurrentWeapon();
            SafeIndent();
            ActivateCurrentWeapon();
        }


        //STAT RELATED
        //HEALTH RELATED

        public void TakeDamage(int _damageTaken)
        {
            HealthPoint = Mathf.Clamp(HealthPoint - _damageTaken, 0, PlayerData.Health);
            if (HealthPoint <= 0)
            {
                StartCoroutine(KillPlayer());
            }
        }

        IEnumerator KillPlayer()
        {
            //TODO: Animator & Audio

            //SceneManager
            return null;
        }

        public void Heal(int _healAmount)
        {
        }

        //GAME LOGIC
        private void Update()
        {
            PlayerMovementUpdate();
            PlayerRotationUpdate();
            PlayerWeaponAnimationUpdate();
            PlayerSkillUpdate();
            PlayerRaycast();
        }


        private void FixedUpdate()
        {
            //Give Directional Smoothed Velocity
            if (PlayerCanMove)
                PlayerRigidbody.velocity = movement * Velocity;
        }

        #region Updates
        private void PlayerMovementUpdate()
        {
            //Retrieve Movement Input
            float mH = Input.GetAxisRaw("Horizontal");
            float mV = Input.GetAxisRaw("Vertical");

            // Calculate absolute movement
            float absoluteMovement = Mathf.Max(Mathf.Abs(mH), Mathf.Abs(mV));

            // Calculate smoothed movement
            float magnitude = 0f;
            if (absoluteMovement > prevMagnitude)
                magnitude = Mathf.Min(1.0f, prevMagnitude + Time.deltaTime / movementSmoother);
            else if (absoluteMovement < prevMagnitude)
                magnitude = Mathf.Max(0f, prevMagnitude - Time.deltaTime / movementSmoother);
            else
                magnitude = absoluteMovement;

            prevMagnitude = magnitude;

            // Computing movement in world space
            movement = Vector3.ClampMagnitude(new Vector3(mH, 0, mV), 1.0f) * magnitude;
        }

        private void PlayerRotationUpdate()
        {
            //Get the Screen positions of the object
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out distanceFromCamera))
            {
                Vector3 worldPosition = ray.GetPoint(distanceFromCamera);
                Vector3 toMouse = worldPosition - this.PlayerModel.transform.position;
                angle = Vector3.SignedAngle(toMouse, this.PlayerModel.transform.forward, Vector3.up);

                // Calculate Variation Angle
                float variationAngle = angle > 0 ? Time.deltaTime * AngularVelocity : -Time.deltaTime * AngularVelocity;
                variationAngle = Mathf.Abs(variationAngle) > Mathf.Abs(angle) ? angle : variationAngle;
                variationAngle = Mathf.Abs(angle) > angularTreshold ? variationAngle : 0;

                // Finally Rotate the model
                this.PlayerModel.transform.Rotate(Vector3.up, -variationAngle);
            }
        }

        private void PlayerWeaponAnimationUpdate()
        {
            modelMovement = PlayerModel.transform.InverseTransformDirection(movement);
            if (PlayerCanMove)
                Weapons[CurrentWeapon].AnimateMovement(PlayerAnimator, modelMovement, angle);
        }

        private void PlayerSkillUpdate()
        {
            //Check If one of the condition for a skill trigger is met.
            var list = PlayerData.WeaponHolder.GetWeaponDataAt(CurrentWeapon).Skills;
            for (var index = 0; index < list.Count; index++)
            {
                var skill = list[index];
                if (skill.Trigger.IsTriggered())
                    AttemptSkill(index);
            }
        }
        #endregion

        #region Animation
        public void CanMove(bool _canMove)
        {
            PlayerCanMove = _canMove;
        }

        public void ResetTriggersAnimator()
        {
            Animations.ResetTriggers(PlayerAnimator);
        }

        public bool IsAnimationLastAtLeast(float _animationTime, int layer)
        {
            return (PlayerAnimator.GetCurrentAnimatorStateInfo(layer).normalizedTime >= _animationTime);
        }

        public bool hasAnimatorPlaying(EAnimation animation, int layer)
        {
            return PlayerAnimator.GetCurrentAnimatorStateInfo(layer).IsName(Animations.GetStringEquivalent(animation));
        }

        public void SetTriggerAnimator(EAnimation _animationName)
        {
            PlayerAnimator.SetTrigger(Animations.GetStringEquivalent(_animationName));
        }

        #endregion

        public void PlayerRaycast()
        {
            Vector3 playerPos = this.PlayerModel.transform.position + new Vector3(0, 0.1f, 0);
            Vector3 cameraPos = Camera.main.transform.position;
            Vector3 dir = (playerPos - cameraPos);
            dir.Normalize();
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (!Physics.Raycast(cameraPos, dir, out hit, 50))
            {
                Debug.Log("No Hit");
                return;
            }
            if (hit.collider == PlayerCollider) this.ctx.playerOccluded = false;
            else this.ctx.playerOccluded = true;
        }

        //GETTER SETTER
        public GameObject Model => PlayerModel;

        public List<Transform> GetCastPoint()
        {
            return Weapons[CurrentWeapon].GetCastPoint();
        }
    }
}