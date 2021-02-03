using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace NextOne
{
    public class Weapon
    {
        public GameObject WeaponModel;
        public int WeaponDamage;
        public const string CASTPOINT = "CastPoint";
        public PlayerController PlayerController;
        public bool OnTriggerActive = false;

        //private List<GameObject> WeaponHit;
        public Lister<GameObject> WeaponHit;

        //private List<AudioClip> WeaponHitSfx;
        public Lister<AudioClip> WeaponHitSfx;

        public bool HasOnHitEffect;
        public bool HasOnHitSfx;

        public WeaponCollider MWeaponCollider;

        public Weapon(GameObject _weaponModel, int _weaponDamage, Lister<GameObject> _weaponHit,
            Lister<AudioClip> _audioClip)
        {
            WeaponModel = _weaponModel;
            _weaponModel.gameObject.AddComponent<WeaponCollider>();
            MWeaponCollider = _weaponModel.GetComponent<WeaponCollider>();
            MWeaponCollider.Init(this);

            WeaponDamage = _weaponDamage;
            WeaponHit = _weaponHit;
            WeaponHitSfx = _audioClip;
            HasOnHitEffect = true;
            HasOnHitSfx = true;
        }

        public Weapon(GameObject _weaponModel, int _weaponDamage, Lister<GameObject> _weaponHit)
        {
            WeaponModel = _weaponModel;
            _weaponModel.gameObject.AddComponent<WeaponCollider>();
            MWeaponCollider = _weaponModel.GetComponent<WeaponCollider>();
            MWeaponCollider.Init(this);
            WeaponDamage = _weaponDamage;
            WeaponHit = _weaponHit;
            WeaponHitSfx = new Lister<AudioClip>();
            HasOnHitEffect = true;
            HasOnHitSfx = true;
        }


        public Weapon(GameObject _weaponModel, int _weaponDamage)
        {
            WeaponModel = _weaponModel;
            _weaponModel.gameObject.AddComponent<WeaponCollider>();
            MWeaponCollider = _weaponModel.GetComponent<WeaponCollider>();
            MWeaponCollider.Init(this);
            WeaponDamage = _weaponDamage;
            WeaponHit = new Lister<GameObject>();
            WeaponHitSfx = new Lister<AudioClip>();
            HasOnHitEffect = false;
            HasOnHitSfx = false;
        }

        public void DamageIfDamageable(Collider _collision)
        {
            Debug.Log("inside weapon damage check");
            var damageableComponent = _collision.gameObject.GetComponentInParent(typeof(IDamageable));

            //If this is  a damageable entity
            if (!damageableComponent) return;
            //If this is an enemy
            if (!(damageableComponent is EnemyController enemyController)) return;

            enemyController.TakeDamage(Damage);
        }

        public void PlaySfx(AudioClip _audioClip)
        {
            //GetComponent<AudioSource>().PlayOneShot(_audioClip);
        }

        public void ActiveTrigger(bool _active)
        {
            OnTriggerActive = _active;
        }

        public Transform GetCastPoint()
        {
            return Model.GetComponentInChildren<CastPoint>().transform;
        }


        public GameObject Model => WeaponModel;

        public int Damage => WeaponDamage;

        public Lister<GameObject> Hit => WeaponHit;

        public Lister<AudioClip> HitSfx => WeaponHitSfx;

        public bool HasHitEffect => HasOnHitEffect;

        public bool HasHitSfx => HasOnHitSfx;
    }
}