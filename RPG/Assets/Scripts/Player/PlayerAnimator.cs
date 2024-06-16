﻿using System;
using Assets.Scripts.Logic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerAnimator : MonoBehaviour, IAnimationStateReader
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private CharacterController _characterController;

        private static readonly int Move = Animator.StringToHash("Walking");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int ComboState = Animator.StringToHash("ComboState");

        private readonly int _idleStateHash = Animator.StringToHash("Idle");
        private readonly int _attackStateHash = Animator.StringToHash("Attack");
        private readonly int _walkingStateHash = Animator.StringToHash("Run");
        private readonly int _deathStateHash = Animator.StringToHash("Die");

        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;

        public AnimatorState State { get; private set; }
        public bool IsAttacking => State == AnimatorState.Attack;

        private int _currentComboState = 0;
        private float _lastAttackTime = 0f;
        private float _comboResetTime = 0.7f;

        private void Update()
        {
            _animator.SetFloat(Move, _characterController.velocity.magnitude, 0.1f, Time.deltaTime);

            if (Time.time - _lastAttackTime > _comboResetTime)
            {
                _currentComboState = 0;
                _animator.SetInteger(ComboState, _currentComboState);
            }
        }

        public void PLayHit() =>
            _animator.SetTrigger(Hit);

        public void PlayDeath() =>
            _animator.SetTrigger(Die);

        public void PlayAttack()
        {
            _lastAttackTime = Time.time;

            _currentComboState++;

                if (_currentComboState > 4)
                    _currentComboState = 1;

            _animator.SetInteger(ComboState, _currentComboState);
            _animator.SetTrigger(Attack);
        }

        public void EnteredState(int stateHash)
        {
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void ExitedState(int stateHash)
        {
            StateExited?.Invoke(StateFor(stateHash));
        }

        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state;
            if (stateHash == _idleStateHash)
                state = AnimatorState.Idle;
            else if (stateHash == _attackStateHash)
                state = AnimatorState.Attack;
            else if (stateHash == _walkingStateHash)
                state = AnimatorState.Walking;
            else if (stateHash == _deathStateHash)
                state = AnimatorState.Died;
            else
                state = AnimatorState.Unknown;

            return state;
        }
    }
}
