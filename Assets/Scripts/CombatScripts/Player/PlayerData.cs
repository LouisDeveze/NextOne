using Assets.Scripts.CombatScripts.Weapons;
using UnityEngine;

namespace Assets.Scripts.CombatScripts.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Next One/Player Data")]
    public class PlayerData : ScriptableObject
    {
        [SerializeField] private int PlayerId = 0;
        [SerializeField] private string PlayerName = "default";
        [SerializeField] private string PlayerDescription = "default description";
        [SerializeField] private GameObject PlayerModel;
        [SerializeField] private int PlayerHealth = 1;
        [SerializeField] private float PlayerVelocity = 1f;

        [SerializeField] private WeaponHolder WeaponsHolder;

        public int Id => PlayerId;

        public string Name => PlayerName;

        public string Description => PlayerDescription;

        public GameObject Model => PlayerModel;

        public int Health => PlayerHealth;

        public float Velocity => PlayerVelocity;

        public WeaponHolder WeaponHolder => WeaponsHolder;
    }
}