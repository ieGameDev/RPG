using Assets.Scripts.Logic;
using System;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private EnemyAnimator _enemyAnimator;

        [SerializeField] private float _currentHealth;
        [SerializeField] private float _maxHealth;

        public event Action HealthChanged;

        public float CurrentHealth
        {
            get => _currentHealth;
            set => _currentHealth = value;
        }
        public float MaxHealth
        {
            get => _maxHealth;
            set => _maxHealth = value;
        }


        public void TakeDamage(float damage)
        {
            CurrentHealth -= damage;

            _enemyAnimator.PlayHit();

            HealthChanged?.Invoke();
        }
    }
}
