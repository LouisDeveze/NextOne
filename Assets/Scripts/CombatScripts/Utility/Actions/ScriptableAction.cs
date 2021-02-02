using UnityEngine;

namespace NextOne
{
    public abstract class ScriptableAction : ScriptableObject
    {
        public abstract void PerformAction(GameObject _gameObject);
    }
}