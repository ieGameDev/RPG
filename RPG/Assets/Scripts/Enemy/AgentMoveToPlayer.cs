﻿using Assets.Scripts.Data;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemy
{
    public class AgentMoveToPlayer : MonoBehaviour
    {
        private const float MinimalDistance = 3.5f;

        public NavMeshAgent Agent;

        private Transform _playerTransform;

        public void Construct(Transform playerTransform) => 
            _playerTransform = playerTransform;        

        private void Update()
        {
            if (Initialized() && PlayerNotReached())
                Agent.destination = _playerTransform.position;
        }

        private bool Initialized() =>
            _playerTransform != null;

        private bool PlayerNotReached() =>
            Agent.transform.position.SqrMagnitudeTo(_playerTransform.position) >= MinimalDistance;
    }
}
