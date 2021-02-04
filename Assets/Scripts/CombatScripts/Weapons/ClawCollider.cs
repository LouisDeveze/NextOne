using UnityEngine;

namespace NextOne
{
    public class ClawCollider : MonoBehaviour
    {
        private bool IsActive = false;
        private bool IsTriggering = false;

        private int ClawDamage;

        public void Update()
        {
            if (IsTriggering)
                IsTriggering = false;
        }

        public void OnTriggerEnter(Collider _collider)
        {
            if (!IsActive)
                return;

            Debug.Log("IM HERE ACTIVE");
            
            if (IsTriggering)
                return;
            IsTriggering = true;
            
            
            Debug.Log("IM HERE TRIGGERD");

            DamageIfDamageable(_collider);
        }

        private void DamageIfDamageable(Collider _collider)
        {
            Debug.Log("inside weapon damage check");
            var damageableComponent = _collider.gameObject.GetComponentInParent(typeof(IDamageable));

            //If this is  a damageable entity
            if (!damageableComponent) return;
            //If this is an enemy
            if (!(damageableComponent is PlayerController player)) return;

            ActiveTrigger(false);
            
            Debug.Log("IM HERE BITCH");

            player.TakeDamage(Damage);
        }

        public void ActiveTrigger(bool _active)
        {
            IsActive = _active;
        }

        public int Damage
        {
            get => ClawDamage;
            set => ClawDamage = value;
        }
    }
}