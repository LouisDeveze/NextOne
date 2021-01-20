using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts.CombatScripts
{

    public abstract class Target
    {
        public GameObject Origin;
    }

    public class GameObjectTarget : Target
    {
        public GameObject[] GameObjects;

        public GameObjectTarget(GameObject _origin, GameObject[] _gameObjects)
        {
            
            GameObjects = _gameObjects;
        }
    }

    public class Vector3Target : Target
    {
        public Vector3 Position;

        public Vector3Target(GameObject _origin, Transform _transform)
        {
            Origin = _origin;
            Position = _transform.position;
        }
    }

    public class SelfTarget : Target
    {
        public SelfTarget(GameObject _origin)
        {
            Origin = _origin;
        }
    }
}