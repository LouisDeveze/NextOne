using UnityEngine;

namespace NextOne
{
    public class Weapon : MonoBehaviour
    {
        private GameObject WeaponModel;
        private int WeaponDamage;

        public Weapon(GameObject _weaponModel, int _weaponDamage)
        {
            WeaponModel = _weaponModel;
            WeaponDamage = _weaponDamage;
        }
        
        //TODO: ON COLLISION 

        public void OnStart()
        {
            
        }

        public void Update()
        {
            
        }
        
        public GameObject Model => WeaponModel;
        public int Damage => WeaponDamage;
        
    }
}