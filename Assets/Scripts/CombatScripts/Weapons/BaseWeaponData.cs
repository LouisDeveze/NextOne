using System.Collections.Generic;
using UnityEngine;

namespace NextOne
{
    [CreateAssetMenu(fileName = "BaseWeapon", menuName = "Next One/Weapons/Base Weapon Data")]
    public abstract class BaseWeaponData : ScriptableObject
    {
        [SerializeField] private int WeaponId = 0;
        [SerializeField] private string WeaponName = "default";
        [SerializeField] private string WeaponDescription = "default description";
        [SerializeField] private Sprite WeaponIcon;
        [SerializeField] private List<GameObject> WeaponsModel = new List<GameObject>();

        [SerializeField] private List<AnchorData> WeaponsAnchor = new List<AnchorData>();
        //[SerializeField] private GameObject WeaponModel;

        //[SerializeField] private Transform WeaponAttachmentPoint;

        [SerializeField] private float WeaponDamageMultiplier = 1f;
        [SerializeField] private float WeaponAttackRateMultiplier = 1f;
        [SerializeField] private float WeaponCooldownMultiplier = 1f;

        [SerializeField] private List<SkillData> WeaponSkills = new List<SkillData>();

        //public GameObject InstantiateWeapon(Transform _parent, Transform _weaponAnchorPoint)
        public List<WeaponController> InstantiateWeapon(Transform _parent)
        {
            var models = new List<WeaponController>();

            for (var i = 0; i < Models.Count; i++)
            {
                var model = Instantiate(Models[i]);
                model.transform.SetParent(_parent);
                model.transform.localPosition = Anchors[i].Position;
                model.transform.rotation = Quaternion.identity;
                model.SetActive(false);
                models.Add(new WeaponController(model, DamageMultiplier));
            }
            
            return models;
        }

        public int Id => WeaponId;

        public string Name => WeaponName;

        public string Description => WeaponDescription;

        public Sprite Icon => WeaponIcon;

        //public GameObject Model => WeaponModel;

        public List<GameObject> Models => WeaponsModel;
        public List<AnchorData> Anchors => WeaponsAnchor;

        //public Transform AttachmentPoint => WeaponAttachmentPoint;

        public float DamageMultiplier => WeaponDamageMultiplier;

        public float AttackRateMultiplier => WeaponAttackRateMultiplier;

        public float CooldownMultiplier => WeaponCooldownMultiplier;

        public List<SkillData> Skills => WeaponSkills;
    }
}