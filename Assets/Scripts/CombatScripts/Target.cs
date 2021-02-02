using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace NextOne
{
    public abstract class Target
    {
        public GameObject Origin;
    }

    public class GameObjectTarget : Target
    {
        private EnemyController[] TTargets;

        public GameObjectTarget(GameObject _origin, GameObject[] _gameObjects)
        {
            //TODO: Target Logic
            //TTargets = _gameObjects;
        }

        public EnemyController[] Targets => TTargets;
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

    public class EnemyTarget : Target
    {
        public List<EnemyController> TargetEnemies;

        public EnemyTarget(GameObject _origin, List<EnemyController> _enemies)
        {
            TargetEnemies = _enemies;
            Origin = _origin;
        }

        public EnemyTarget(GameObject _origin)
        {
            TargetEnemies = new List<EnemyController>();
            Origin = _origin;
        }

        public EnemyTarget(GameObject _origin, EnemyController _enemy)
        {
            TargetEnemies = new List<EnemyController>() {_enemy};
            Origin = _origin;
        }

        public void AddTarget(EnemyController _enemy)
        {
            TargetEnemies.Add(_enemy);
        }

        public List<EnemyController> Enemies => TargetEnemies;
    }

    public class NoTarget : Target
    {
    }

    public class SelfTarget : Target
    {
        public SelfTarget(GameObject _origin)
        {
            Origin = _origin;
        }
    }

    public class ForwardTarget : Target
    {
        private Vector3 TargetDirection;

        public ForwardTarget(GameObject _origin)
        {
            Origin = _origin;
            TargetDirection = _origin.transform.forward;
        }

        public Vector3 Direction => TargetDirection;
    }
}