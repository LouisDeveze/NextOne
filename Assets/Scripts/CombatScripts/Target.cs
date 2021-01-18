using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts.CombatScripts
{
    public abstract class Target
    {
    }

    public class GameObjectTarget : Target
    {
        public GameObject[] GameObjects;

        public GameObjectTarget(GameObject[] _gameObjects)
        {
            GameObjects = _gameObjects;
        }
    }

    public class Vector3Target : Target
    {
        public Vector3 Position;

        public Vector3Target(Transform _transform)
        {
            Position = _transform.position;
        }
    }
}