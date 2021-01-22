using UnityEngine;

namespace NextOne
{
    [CreateAssetMenu(fileName = "Anchor", menuName = "Next One/Anchor/Anchor Data")]
    public class AnchorData : ScriptableObject
    {
        [SerializeField] private Vector3 AnchorPosition;

        public Vector3 Position => AnchorPosition;
    }
}