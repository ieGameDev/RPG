using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerDeath : MonoBehaviour
    {
        [SerializeField] private PlayerHealth _health;
        [SerializeField] private PlayerMove _move;
        [SerializeField] private PlayerAttack _attack;
        [SerializeField] private PlayerAnimator _animator;

        [SerializeField] private GameObject _playerDeathFX;
        private bool _isDead;

        private void Start() =>
            _health.HealthChanged += HealthChanged;

        private void OnDestroy() =>
            _health.HealthChanged -= HealthChanged;

        private void HealthChanged()
        {
            if (!_isDead && _health.CurrentHealth <= 0)
                Die();
        }

        private void Die()
        {
            _isDead = true;

            _move.enabled = false;
            _attack.enabled = false;
            _animator.PlayDeath();

            Instantiate(_playerDeathFX, DeadEffectPosition(), Quaternion.identity);
        }

        private Vector3 DeadEffectPosition()
        {
            Vector3 deathEffectPosition = transform.position;
            deathEffectPosition.y += 2f;
            deathEffectPosition.z -= 1f;

            return deathEffectPosition;
        }
    }
}
