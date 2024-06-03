using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    [RequireComponent(typeof(EnemyHealth))]
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyDeath : MonoBehaviour
    {
        [SerializeField] private EnemyHealth _enemyHealth;
        [SerializeField] private EnemyAnimator _enemyAnimator;

        [SerializeField] private GameObject _deathFX;

        public event Action Happend;

        private void Start() =>
            _enemyHealth.HealthChanged += HealthChanged;

        private void OnDestroy() =>
            _enemyHealth.HealthChanged -= HealthChanged;

        private void HealthChanged()
        {
            if (_enemyHealth.CurrentHealth <= 0)
                Die();
        }

        private void Die()
        {
            _enemyHealth.HealthChanged -= HealthChanged;

            _enemyAnimator.PlayDeath();

            SpawnDeadFX();
            StartCoroutine(DestroyTimer());

            Happend?.Invoke();
        }

        private void SpawnDeadFX() => 
            Instantiate(_deathFX, transform.position, Quaternion.identity);

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }
    }
}
