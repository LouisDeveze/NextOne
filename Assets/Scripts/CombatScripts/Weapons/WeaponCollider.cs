using System;
using UnityEngine;
using UnityEngine.VFX;

namespace NextOne
{
    public class WeaponCollider : MonoBehaviour
    {
        private Weapon parent;
        private bool isColliding = false;

        public void Init(Weapon _weapon)
        {
            parent = _weapon;
        }

        void Update()
        {
            isColliding = false;
        }

        void OnTriggerEnter(Collider _collider)
        {
            //   Debug.Log("pre trigger active check");
            if (!parent.OnTriggerActive)
                return;

            if (isColliding)
                return;
            isColliding = true;

            Debug.Log("post trigger active check");

            if (parent.Hit.Count() > 0)
            {
                Vector3 contactPoint = _collider.ClosestPoint(transform.position);
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, transform.position - contactPoint);
                Vector3 position = contactPoint;

                if (!parent.HasHitEffect)
                {
                    int rndHit = 0;
                    if (parent.Hit.Count() > 1)
                    {
                        System.Random rnd = new System.Random();
                        rndHit = rnd.Next(0, parent.Hit.Count());
                    }

                    GameObject hitVfx = GameObject.Instantiate(parent.Hit.ElementAt(rndHit), position, rotation);
                    ParticleSystem ps = hitVfx.GetComponent<ParticleSystem>();
                    if (!ps)
                    {
                        var ve = hitVfx.GetComponent<VisualEffect>();
                        if (ve)
                        {
                            GameObject.Destroy(hitVfx.gameObject, .5f);
                        }
                        else
                        {
                            var psChild = hitVfx.transform.GetChild(0).GetComponent<ParticleSystem>();
                            GameObject.Destroy(hitVfx.gameObject, psChild.main.duration);
                        }
                    }
                    else
                        GameObject.Destroy(hitVfx.gameObject, ps.main.duration);
                }
            }

            if (parent.HitSfx.Count() > 0)
            {
                int rndSfx = 0;
                if (parent.HitSfx.Count() > 1)
                {
                    System.Random rnd = new System.Random();
                    rndSfx = rnd.Next(0, parent.HitSfx.Count());
                }

                parent.PlaySfx(parent.HitSfx.ElementAt(rndSfx));
            }

            parent.DamageIfDamageable(_collider);
        }
    }
}