using System.Collections.Generic;
using Assets.Scripts.CombatScripts.Skills;
using UnityEditor.PackageManager;
using UnityEngine;

//LE SKILL POUR CHANGER
namespace Assets.Scripts.CombatScripts.Weapons
{
    [CreateAssetMenu(fileName = "WeaponHolder", menuName = "Next One/Weapons/Weapon Holder")]
    public class WeaponHolder : ScriptableObject
    {
        [SerializeField] private List<BaseWeaponData> BaseWeaponsData = new List<BaseWeaponData>();
        [SerializeField] private int DefaultHolderWeapon = 0;

        [SerializeField] private float WeaponSwitchCooldown = 1f;

        public List<BaseWeaponData> WeaponsData => BaseWeaponsData;

        public int DefaultWeapon => DefaultHolderWeapon;

        public float SwitchCooldown => WeaponSwitchCooldown;

        public BaseWeaponData GetWeaponDataAt(int _index)
        {
            return _index < WeaponsData.Count ? WeaponsData[_index] : GetWeaponDataAt(0);
        }
    }
}