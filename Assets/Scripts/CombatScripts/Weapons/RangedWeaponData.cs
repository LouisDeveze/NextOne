using UnityEngine;

namespace Assets.Scripts.CombatScripts.Weapons
{
    [CreateAssetMenu(fileName = "RangedWeapon", menuName = "Next One/Weapons/Ranged Weapon Data")]
    public class RangedWeaponData : BaseWeaponData
    {
        [SerializeField] private ProjectileData[] ProjectileData;

        [SerializeField] private int ProjectilesNumberToFire = 1;

        [SerializeField] private Transform ProjectileAttachmentPoint;
    }
}