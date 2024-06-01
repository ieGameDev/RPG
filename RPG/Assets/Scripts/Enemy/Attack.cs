using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class Attack : MonoBehaviour
    {
        public EnemyAnimator Animator;

        public float AttackCooldown = 3f;
        public float Cleavage = 0.5f;
        public float EffectiveDistance = 0.5f;

        private IGameFactory _gameFactory;
        private Transform _playerTransform;
        private float _attackCooldown;
        private bool _isAttacking;
        private int _layerMask;
        private Collider[] _hits = new Collider[1];
        private bool _attackIsActive;

        private void Awake()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();

            _layerMask = 1 << LayerMask.NameToLayer("Player");

            _gameFactory.PlayerCreated += OnPlayerCreated;
        }

        private void Update()
        {
            UpdateCooldown();

            if (CanAttack())
                StartAttack();
        }

        private void OnAttack()
        {
            if (Hit(out Collider hit))
            {
                Debug.Log("Damage");
            }
        }

        private bool Hit(out Collider hit)
        {
            int hitsCount = Physics.OverlapSphereNonAlloc(StartPoint(), Cleavage, _hits, _layerMask);
            hit = _hits.FirstOrDefault();
            return hitsCount > 0;
        }

        private void OnAttackEnded()
        {
            _attackCooldown = AttackCooldown;
            _isAttacking = false;
        }

        public void DisableAttack() =>
            _attackIsActive = false;

        public void EnableAttack() =>
            _attackIsActive = true;

        private Vector3 StartPoint() =>
            new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) + transform.forward * EffectiveDistance;

        private void UpdateCooldown()
        {
            if (!CooldownIsUp())
                _attackCooldown -= Time.deltaTime;
        }

        private bool CanAttack() =>
           _attackIsActive && !_isAttacking && CooldownIsUp();

        private bool CooldownIsUp() =>
            _attackCooldown <= 0;

        private void StartAttack()
        {
            transform.LookAt(_playerTransform);
            Animator.PlayAttack();

            _isAttacking = true;
        }

        private void OnPlayerCreated() =>
            _playerTransform = _gameFactory.PlayerGameObject.transform;
    }
}
