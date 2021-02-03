using System.Collections.Generic;
using UnityEngine;

namespace NextOne
{
    [CreateAssetMenu(fileName = "SpawnAndDestroyAction", menuName = "Next One/Actions/Spawn and Destroy Action")]
    public class SpawnAndDestroyAction : ScriptableAction
    {
        [SerializeField] private List<GameObject> ObjectsToSpawnAndDestroys;
        [SerializeField] private float DestroyDelay;
        [SerializeField] private Vector3 Offset;

        public override void PerformAction(GameObject _gameObject)
        {
            foreach (GameObject objectAction in ObjectsToSpawnAndDestroys)
            {
                GameObject clone = Instantiate(objectAction, _gameObject.transform.position + Offset,
                    _gameObject.transform.rotation);
                Destroy(clone, DestroyDelay);
            }
        }
    }
}