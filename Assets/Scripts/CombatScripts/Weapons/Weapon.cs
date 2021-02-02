using System.Collections.Generic;
using UnityEngine;

namespace NextOne
{
    public class Weapon 
    {
        private GameObject WeaponModel;
        private int WeaponDamage;
        private const string CASTPOINT = "CastPoint";
        private PlayerController PlayerController;
        private bool OnTriggerActive = false;

        //private List<GameObject> WeaponHit;
        private Lister<GameObject> WeaponHit;

        //private List<AudioClip> WeaponHitSfx;
        private Lister<AudioClip> WeaponHitSfx;

        private bool HasOnHitEffect;
        private bool HasOnHitSfx;

        public Weapon(GameObject _weaponModel, int _weaponDamage, Lister<GameObject> _weaponHit,
            Lister<AudioClip> _audioClip)
        {
            WeaponModel = _weaponModel;
            WeaponDamage = _weaponDamage;
            WeaponHit = _weaponHit;
            WeaponHitSfx = _audioClip;
            HasOnHitEffect = true;
            HasOnHitSfx = true;
        }   
        
        public Weapon(GameObject _weaponModel, int _weaponDamage, Lister<GameObject> _weaponHit)
        {
            WeaponModel = _weaponModel;
            WeaponDamage = _weaponDamage;
            WeaponHit = _weaponHit;
            WeaponHitSfx = new Lister<AudioClip>();
            HasOnHitEffect = true;
            HasOnHitSfx = true;
        }
        
        

        public Weapon(GameObject _weaponModel, int _weaponDamage)
        {
            WeaponModel = _weaponModel;
            WeaponDamage = _weaponDamage;
            WeaponHit = new Lister<GameObject>();
            WeaponHitSfx = new Lister<AudioClip>();
            HasOnHitEffect = false;
            HasOnHitSfx = false;
        }


        //TODO: ON COLLISION 

        void OnCollisionEnter(Collision _collision)
        {
            if (!HasHitEffect)
                return;

            if (!OnTriggerActive)
                return;

            if (Hit.Count() > 0)
            {
                ContactPoint contactPoint = _collision.GetContact(0);
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contactPoint.normal);
                Vector3 position = contactPoint.point;

                int rndHit = 0;
                if (Hit.Count() > 1)
                {
                    System.Random rnd = new System.Random();
                    rndHit = rnd.Next(0, Hit.Count());
                }


                GameObject hitVfx = GameObject.Instantiate(Hit.ElementAt(rndHit), position, rotation);
                ParticleSystem ps = hitVfx.GetComponent<ParticleSystem>();
                if (!ps)
                {
                    var psChild = hitVfx.transform.GetChild(0).GetComponent<ParticleSystem>();
                    GameObject.Destroy(hitVfx, psChild.main.duration);
                }
                else
                    GameObject.Destroy(hitVfx, ps.main.duration);
            }

            if (HitSfx.Count() > 0)
            {
                int rndSfx = 0;
                if (HitSfx.Count() > 1)
                {
                    System.Random rnd = new System.Random();
                    rndSfx = rnd.Next(0, HitSfx.Count());
                }

                PlaySfx(HitSfx.ElementAt(rndSfx));
            }


            DamageIfDamageable(_collision);
        }

        private void DamageIfDamageable(Collision _collision)
        {
            var damageableComponent = _collision.gameObject.GetComponentInParent(typeof(IDamageable));

            //If this is  a damageable entity
            if (!damageableComponent) return;
            //If this is an enemy
            if (!(damageableComponent is EnemyController enemyController)) return;

            enemyController.TakeDamage(Damage);
        }

        private void PlaySfx(AudioClip _audioClip)
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