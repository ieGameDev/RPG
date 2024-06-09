using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Logic;
using System;
using UnityEngine;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(PlayerAnimator))]
    public class PlayerHealth : MonoBehaviour, ISavedProgress, IHealth
    {
        [SerializeField] private PlayerAnimator _playerAnimator;

        private PlayerState _state;

        public event Action HealthChanged;

        public float CurrentHealth
        {
            get => _state?.CurrentHP ?? 0;
            set
            {
                if (_state.CurrentHP != value)
                {
                    _state.CurrentHP = value;
                    HealthChanged?.Invoke();
                }
            }
        }
        public float MaxHealth
        {
            get => _state?.MaxHP ?? 0;
            set
            {
                if (_state != null)
                {
                    _state.MaxHP = value;
                }
            }
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _state = progress.PlayerState;
            HealthChanged?.Invoke();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.PlayerState.CurrentHP = CurrentHealth;
            progress.PlayerState.MaxHP = MaxHealth;
        }

        public void TakeDamage(float damage)
        {
            if (CurrentHealth <= 0)
                return;

            CurrentHealth -= damage;
            _playerAnimator.PLayHit();

            HealthChanged?.Invoke();
        }

        public void Initialize(PlayerState initialState)
        {
            _state = initialState;
        }
    }
}
