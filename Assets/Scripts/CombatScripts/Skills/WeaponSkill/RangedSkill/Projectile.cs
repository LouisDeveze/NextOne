using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.CombatScripts.Enemies;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Skills.WeaponSkill.RangedSkill
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private GameObject PSource;

        [SerializeField] private int ProjectileDamage;
        [SerializeField] private float ProjectileDestroyDelay;

        [SerializeField] private GameObject ProjectileHit;

        [SerializeField] private List<GameObject> ProjectileTrails;

        [SerializeField] private int ProjectileMaxCollision;
        private int CurrentCollisionNumber = 0;

        private bool EndOfLife = false;

        public Projectile(GameObject _source, int _projectileDamage,
            float _projectileDestroyDelay, GameObject _projectileHit)
        {
            PSource = _source;
            ProjectileDamage = _projectileDamage;
            ProjectileDestroyDelay = _projectileDestroyDelay;
            Hit = _projectileHit;
        }

        void OnCollisionEnter(Collision _collision)
        {
            var layerCollidedWith = _collision.gameObject.transform.parent.gameObject.layer;
            if (!Source || layerCollidedWith == Source.layer ||
                layerCollidedWith == LayerMask.NameToLayer("VFX")) return;

            CurrentCollisionNumber++;

            if (CurrentCollisionNumber == MaxCollision)
                EndOfLife = true;

            if (Hit)
            {
                ContactPoint contactPoint = _collision.GetContact(0);
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contactPoint.normal);
                Vector3 position = contactPoint.point;

                var HitVFX = Instantiate(Hit, position, rotation) as GameObject;

                var ps = HitVFX.GetComponent<ParticleSystem>();
                //If NO PS directly attached
                if (!ps)
                {
                    var psChild = HitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(HitVFX, psChild.main.duration);
                }
                else
                    Destroy(HitVFX, ps.main.duration);
            }

            DamageIfDamageable(_collision);

            if (EndOfLife)
                StartCoroutine(DestroyParticle(.5f));
        }

        private void DamageIfDamageable(Collision _collision)
        {
            var damageableComponent = _collision.gameObject.GetComponentInParent(typeof(IDamageable));

            if (!damageableComponent) return;
            if (damageableComponent is EnemyController enemyController)
                enemyController.TakeDamage(Damage);
        }

        //Destroy ATTACHED particles
        public IEnumerator DestroyParticle(float _waitTime)
        {
            if (Trails.Count > 0)
            {
                foreach (var t in Trails)
                {
                    t.transform.parent = null;
                    var ps = t.GetComponent<ParticleSystem>();
                    if (ps == null) continue;
                    ps.Stop();
                    Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
                }
            }

            if (transform.childCount > 0 && _waitTime != 0)
            {
                var tList = new List<Transform>();
                foreach (Transform t in transform.GetChild(0).transform)
                {
                    tList.Add(t);
                }

                while (transform.GetChild(0).localScale.x > 0)
                {
                    yield return new WaitForSeconds(0.01f);
                    transform.GetChild(0).localScale -= new Vector3(.1f, .1f, .1f);
                    foreach (var t in tList)
                    {
                        t.localScale -= new Vector3(.1f, .1f, .1f);
                    }
                }
            }

            yield return new WaitForSeconds(_waitTime);
            Destroy(gameObject, DestroyDelay);
        }

        public GameObject Source
        {
            get => PSource;
            set => PSource = value;
        }

        public int Damage
        {
            get => ProjectileDamage;
            set => ProjectileDamage = value;
        }

        public float DestroyDelay
        {
            get => ProjectileDestroyDelay;
            set => ProjectileDestroyDelay = value;
        }

        public GameObject Hit
        {
            get => ProjectileHit;
            set => ProjectileHit = value;
        }

        public List<GameObject> Trails
        {
            get => ProjectileTrails;
            set => ProjectileTrails = value;
        }

        public int MaxCollision
        {
            get => ProjectileMaxCollision;
            set => ProjectileMaxCollision = value;
        }
    }
}