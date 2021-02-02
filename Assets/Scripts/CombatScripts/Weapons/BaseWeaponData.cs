using System.Collections.Generic;
using UnityEngine;

namespace NextOne
{
    public enum EWeaponAnimation
    {
        None = 0,
        OneHandedMelee = 1,
        TwoHandedMelee = 2,
        OneHandedRanged = 3,
        TwoHandedRanged = 4,
        TwoHandedMix = 5
    }

    [CreateAssetMenu(fileName = "BaseWeapon", menuName = "Next One/Weapons/Base Weapon Data")]
    public abstract class BaseWeaponData : ScriptableObject
    {
        [SerializeField] private int WeaponId = 0;
        [SerializeField] private string WeaponName = "default";
        [SerializeField] private string WeaponDescription = "default description";
        [SerializeField] private Sprite WeaponIcon;
        [SerializeField] private List<GameObject> WeaponsModel = new List<GameObject>();
        [SerializeField] private bool UniformWeaponDamage = false;
        [SerializeField] private List<int> WeaponDamages = new List<int>();

        [SerializeField] private bool HasHit = false;
        [SerializeField] private bool UniformWeaponHitPrefab = false;

        //[SerializeField] private List<List<GameObject>> WeaponsHitPrefab = new List<List<GameObject>>();
        [SerializeField] private List<Lister<GameObject>> WeaponsHitPrefab = new List<Lister<GameObject>>();

        [SerializeField] private bool UniformWeaponHitSfx = false;

        //[SerializeField] private List<List<AudioClip>> WeaponsHitSfx = new List<List<AudioClip>>();
        [SerializeField] private List<Lister<AudioClip>> WeaponsHitSfx = new List<Lister<AudioClip>>();


        // [SerializeField] private List<AnchorData> WeaponsAnchor = new List<AnchorData>();

        [SerializeField] private EWeaponAnimation WeaponAnimationType;

        [SerializeField] private AnimatorOverrideController WeaponAnimator;

        [SerializeField] private float WeaponDamageMultiplier = 1f;
        [SerializeField] private float WeaponAttackRateMultiplier = 1f;
        [SerializeField] private float WeaponCooldownMultiplier = 1f;

        [SerializeField] private List<SkillData> WeaponSkills = new List<SkillData>();

        //public GameObject InstantiateWeapon(Transform _parent, Transform _weaponAnchorPoint)
        public WeaponController InstantiateWeapon(List<WeaponAnchors> _weaponAnchorsList)
        {
            var models = new List<Weapon>();

            for (var i = 0; i < Models.Count; i++)
            {
                var model = Instantiate(Models[i], _weaponAnchorsList[i].transform);
                model.SetActive(false);

                if (!HasHit)
                {
                    models.Add(UniformDamage ? new Weapon(model, Damages[0]) : new Weapon(model, Damages[i]));
                }
                else
                {
                    //If Same damage for all weapons
                    if (UniformDamage)
                    {
                        //If Same VFX for all weapons
                        if (UniformWeaponHitPrefab)
                        {
                            if (WeaponsHitSfx.Count > 0)
                                //If Same SFX for all weapons
                                models.Add(UniformWeaponHitSfx
                                    ? new Weapon(model, Damages[0], WeaponsHitPrefab[0], WeaponsHitSfx[0])
                                    : new Weapon(model, Damages[0], WeaponsHitPrefab[0], WeaponsHitSfx[i]));
                            else
                                models.Add(new Weapon(model, Damages[0], WeaponsHitPrefab[0]));
                        }
                        //If not same vfx for all weapons
                        else
                        {
                            if (WeaponsHitSfx.Count > 0)
                                //If Same SFX for all weapons
                                models.Add(UniformWeaponHitSfx
                                    ? new Weapon(model, Damages[0], WeaponsHitPrefab[i], WeaponsHitSfx[0])
                                    : new Weapon(model, Damages[0], WeaponsHitPrefab[i], WeaponsHitSfx[i]));
                            else
                                models.Add(new Weapon(model, Damages[0], WeaponsHitPrefab[i]));
                        }
                    }
                    //If not same dmg for all weapons
                    else
                    {
                        //If Same VFX for both weapons
                        if (UniformWeaponHitPrefab)
                        {
                            if (WeaponsHitSfx.Count > 0)
                                //If Same SFX for both weapons
                                models.Add(UniformWeaponHitSfx
                                    ? new Weapon(model, Damages[i], WeaponsHitPrefab[0], WeaponsHitSfx[0])
                                    : new Weapon(model, Damages[i], WeaponsHitPrefab[0], WeaponsHitSfx[i]));
                            else
                                models.Add(new Weapon(model, Damages[i], WeaponsHitPrefab[0]));
                        }
                        //If not same vfx for both weapons
                        else
                        {
                            if (WeaponsHitSfx.Count > 0)
                                //If Same SFX for both weapons
                                models.Add(UniformWeaponHitSfx
                                    ? new Weapon(model, Damages[i], WeaponsHitPrefab[i], WeaponsHitSfx[0])
                                    : new Weapon(model, Damages[i], WeaponsHitPrefab[i], WeaponsHitSfx[i]));
                            else
                                models.Add(new Weapon(model, Damages[i], WeaponsHitPrefab[i]));
                        }
                    }
                }
            }

            return new WeaponController(models, WeaponAnimation, WeaponAnimator);
        }

        public int Id => WeaponId;

        public string Name => WeaponName;

        public string Description => WeaponDescription;

        public Sprite Icon => WeaponIcon;

        //public GameObject Model => WeaponModel;

        public List<GameObject> Models => WeaponsModel;
        //  public List<AnchorData> Anchors => WeaponsAnchor;

        //public Transform AttachmentPoint => WeaponAttachmentPoint;

        public float DamageMultiplier => WeaponDamageMultiplier;

        public float AttackRateMultiplier => WeaponAttackRateMultiplier;

        public float CooldownMultiplier => WeaponCooldownMultiplier;

        public List<SkillData> Skills => WeaponSkills;

        public AnimatorOverrideController Animator => WeaponAnimator;

        public EWeaponAnimation WeaponAnimation => WeaponAnimationType;

        public List<int> Damages => WeaponDamages;

        public bool UniformDamage => UniformWeaponDamage;
    }
}