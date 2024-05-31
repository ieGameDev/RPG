using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemy
{
    public class AgentMoveToPlayer : MonoBehaviour
    {
        private const float MinimalDistance = 3.5f;

        public NavMeshAgent Agent;

        private Transform _playerTransform;
        private IGameFactory _gameFactory;

        private void Start()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();

            if (_gameFactory.PlayerGameObject != null)
                InitializePlayerTransform();
            else
                _gameFactory.PlayerCreated += PlayerCreated;
        }

        private void Update()
        {
            if (Initialized() && PlayerNotReached())
                Agent.destination = _playerTransform.position;
        }

        private bool Initialized() => 
            _playerTransform != null;

        private void PlayerCreated() =>
            InitializePlayerTransform();

        private void InitializePlayerTransform() =>
            _playerTransform = _gameFactory.PlayerGameObject.transform;

        private bool PlayerNotReached() =>
            Vector3.Distance(Agent.transform.position, _playerTransform.position) >= MinimalDistance;
    }
}
