using UnityEngine;

namespace Assets.Scripts.CombatScripts.Weapons
{
    [CreateAssetMenu(fileName = "Anchor", menuName = "Next One/Anchor/Anchor Data")]
    public class AnchorData : ScriptableObject
    {
        [SerializeField] private Vector3 AnchorPosition;

        public Vector3 Position => AnchorPosition;
    }
}