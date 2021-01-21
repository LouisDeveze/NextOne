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

        public Projectile(GameObject _source, int _projectileDamage,
            float _projectileDestroyDelay)
        {
            PSource = _source;
            ProjectileDamage = _projectileDamage;
            ProjectileDestroyDelay = _projectileDestroyDelay;
        }

        void OnCollisionEnter(Collision _collision)
        {
            var layerCollidedWith = _collision.gameObject.transform.parent.gameObject.layer;
            if (Source && layerCollidedWith != Source.layer
                       && layerCollidedWith != LayerMask.NameToLayer("VFX"))
                DamageIfDamageable(_collision);
        }

        private void DamageIfDamageable(Collision _collision)
        {
            Component damageableComponent = _collision.gameObject.GetComponentInParent(typeof(IDamageable));
            /*  _collision.gameObject.transform.gameObject.
                  .GetComponent(typeof(IDamageable));*/
            //_collision.collider.transform.parent.GetComponent(typeof(IDamageable));

            if (damageableComponent)
            {
                if (damageableComponent is EnemyController enemyController)
                    enemyController.TakeDamage(Damage);
            }

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
    }
}