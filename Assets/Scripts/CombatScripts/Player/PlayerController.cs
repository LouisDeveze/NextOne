using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NextOne
{
    public class PlayerController : MonoBehaviour, IDamageable
    {
        //TODO: Game Logic

        [SerializeField] private PlayerData PlayerData;
        [SerializeField] private Transform CastAnchorPoint;

        //Player Stats
        private int HealthPoint;
        private float Velocity;

        //Weapon Stats
        private List<List<WeaponController>> Weapons = new List<List<WeaponController>>();
        private int CurrentWeapon = 0;
        private bool IsCasting = false;

        //Skills Script References
        private List<ISkill> Skills = new List<ISkill>();

        private void Start()
        {
            if (PlayerData != null)
                LoadPlayer(PlayerData);
        }

        private void LoadPlayer(PlayerData _playerData)
        {
            //Load & Configure Player Model 
            GameObject model = Instantiate(_playerData.Model);
            model.transform.SetParent(transform);
            model.transform.localPosition = Vector3.zero;
            model.transform.rotation = Quaternion.identity;

            //Attach Component
            AttachComponents();

            //Set Weapon Stats
            CurrentWeapon = _playerData.WeaponHolder.DefaultWeapon;

            foreach (var baseWeaponData in _playerData.WeaponHolder.WeaponsData)
            {
                Weapons.Add(baseWeaponData.InstantiateWeapon(model.transform));
                /*Weapons.Add(new WeaponController(
                    baseWeaponData.InstantiateWeapon(model.transform),
                    baseWeaponData.DamageMultiplier));*/
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
            //TODO: check cooldown
            var skillParams = new SkillUseParams {Origin = gameObject};
            Skills[_index].Use(skillParams);
        }

        //WEAPON RELATED

        private void ActivateCurrentWeapon()
        {
            foreach (var weapon in Weapons[CurrentWeapon])
            {
                weapon.SetActive(true);
            }

            AttachSkills();
        }

        private void DeactivateUnfocusedWeapon()
        {
            for (int i = 0; i < Weapons.Count; i++)
            {
                if (i == CurrentWeapon)

                    return;

                for (int j = 0; j < Weapons[i].Count; j++)
                {
                    Weapons[i][j].SetActive(false);
                }
            }
        }

        private void DeactivateCurrentWeapon()
        {
            foreach (var weapon in Weapons[CurrentWeapon])
            {
                weapon.SetActive(false);
            }

            DetachSkills();
        }

        private void SafeIndent()
        {
            if (CurrentWeapon + 1 < Weapons.Count)
                CurrentWeapon += 1;
            else CurrentWeapon = 0;
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
            //Check If one of the condition for a skill trigger is met.
            var list = PlayerData.WeaponHolder.GetWeaponDataAt(CurrentWeapon).Skills;
            for (var index = 0; index < list.Count; index++)
            {
                var skill = list[index];
                if (skill.Trigger.IsTriggered())
                    AttemptSkill(index);
            }
        }
    }
}