using UnityEngine;

namespace Assets.Scripts.CombatScripts.Weapons
{
    [CreateAssetMenu(fileName = "RangedWeapon", menuName = "Next One/Weapons/Ranged Weapon Data")]
    public class RangedWeaponData : BaseWeaponData
    {
        [SerializeField] private Transform ProjectileAttachmentPoint;
    }
}