using UnityEngine;

namespace NextOne
{
    public class WeaponController : MonoBehaviour
    {
        private GameObject WeaponModel;
        private float WeaponDamage;

        public WeaponController(GameObject _weaponModel, float _weaponDamage)
        {
            WeaponModel = _weaponModel;
            WeaponDamage = _weaponDamage;
        }

        public void SetActive(bool _active)
        {
            WeaponModel.SetActive(_active);
        }
    }
}