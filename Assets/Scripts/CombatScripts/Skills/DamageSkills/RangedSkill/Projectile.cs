using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace NextOne
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private GameObject PSource;

        [SerializeField] private int ProjectileDamage;
        [SerializeField] private float ProjectileDestroyDelay;

        [SerializeField] private GameObject ProjectileHit;

        [SerializeField] private List<GameObject> ProjectileTrails;

        [SerializeField] private float ProjectileVelocity;
        [SerializeField] private float ProjectileAccuracy;

        [SerializeField] private int ProjectileMaxCollision;
        [SerializeField] private AudioClip ProjectileHitSfx;
        [SerializeField] private Vector3 ProjectileDirection;


        private Vector3 offset;

        private int CurrentCollisionNumber = 0;
        private bool EndOfLife = false;

        public void OnStart()
        {
            OnInitialization();
        }

        private void OnInitialization()
        {
            var rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            if (!(Accuracy < 100f)) return;
            Accuracy = 1 - (Accuracy / 100);

            for (int i = 0; i < 2; i++)
            {
                var val = 1 * Random.Range(-Accuracy, Accuracy);
                var index = Random.Range(0, 2);
                if (i == 0)
                {
                    offset = index == 0 ? new Vector3(0, -val, 0) : new Vector3(0, val, 0);
                }
                else
                {
                    offset = index == 0 ? new Vector3(0, offset.y, -val) : new Vector3(0, offset.y, val);
                }
            }
        }

        public void FixedUpdate()
        {
            transform.LookAt(Vector3.Normalize(Direction));

            if (Velocity != 0 && transform != null)
                transform.position += (Direction + offset) * Velocity * Time.deltaTime;
        }

        void OnCollisionEnter(Collision _collision)
        {
            var layerCollidedWith = _collision.gameObject.transform.parent.gameObject.layer;

            if (!Source)
                return;

            if (layerCollidedWith == Source.layer
                || layerCollidedWith == LayerMask.NameToLayer("VFX")
                || layerCollidedWith == LayerMask.NameToLayer("Weapons"))
                return;

            CurrentCollisionNumber++;

            if (CurrentCollisionNumber == MaxCollision)
            {
                EndOfLife = true;
                Velocity = 0;
                GetComponent<Rigidbody>().isKinematic = true;
            }

            if (Hit)
            {
                ContactPoint contactPoint = _collision.GetContact(0);
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contactPoint.normal);
                Vector3 position = contactPoint.point;

                var hitVfx = Instantiate(Hit, position, rotation) as GameObject;
                Debug.Log("Hit Instantiated in: " + this.GetInstanceID());

                var ps = hitVfx.GetComponent<ParticleSystem>();
                //If NO PS directly attached
                if (!ps)
                {
                    var ve = hitVfx.GetComponent<VisualEffect>();
                    ve.Play();

                    if (ve)
                    {
                        Destroy(hitVfx.gameObject, DestroyDelay);
                    }
                    else
                    {
                        var psChild = hitVfx.transform.GetChild(0).GetComponent<ParticleSystem>();
                        Destroy(hitVfx.gameObject, psChild.main.duration);
                    }
                }
                else
                    Destroy(hitVfx.gameObject, ps.main.duration);
            }

            if (HitSfx)
                PlaySfx(HitSfx);

            DamageIfDamageable(_collision);

            if (EndOfLife)
                StartCoroutine(DestroyParticle(.5f));
            //Destroy(gameObject);
        }

        private void DamageIfDamageable(Collision _collision)
        {
            var damageableComponent = _collision.gameObject.GetComponentInParent(typeof(IDamageable));

            //If this is  a damageable entity
            if (!damageableComponent) return;
            //If this is an enemy
            if (!(damageableComponent is EnemyController enemyController)) return;

            enemyController.TakeDamage(Damage);
            Debug.Log("Projectile: " + this.GetInstanceID()
                                     + " Damaged: " + enemyController.GetInstanceID());
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

        public void PlaySfx(AudioClip _audioClip)
        {
            GetComponent<AudioSource>().PlayOneShot(_audioClip);
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

        public AudioClip HitSfx
        {
            get => ProjectileHitSfx;
            set => ProjectileHitSfx = value;
        }

        public float Velocity
        {
            get => ProjectileVelocity;
            set => ProjectileVelocity = value;
        }

        public float Accuracy
        {
            get => ProjectileAccuracy;
            set => ProjectileAccuracy = value;
        }

        public Vector3 Direction
        {
            get => ProjectileDirection;
            set => ProjectileDirection = value;
        }
    }
}