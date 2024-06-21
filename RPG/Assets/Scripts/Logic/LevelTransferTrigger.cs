using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.States;
using UnityEngine;

namespace Assets.Scripts.Logic
{
    public class LevelTransferTrigger : MonoBehaviour
    {
        private const string PlayerTag = "Player";

        [SerializeField] private string _nextLevel;

        private IStateMachine _stateMachine;
        private bool _triggered;

        private void Awake() => 
            _stateMachine = AllServices.Container.Single<IStateMachine>();

        private void OnTriggerEnter(Collider other)
        {
            if (_triggered)
                return;

            if (other.CompareTag(PlayerTag))
            {
                _stateMachine.Enter<LoadLevelState, string>(_nextLevel);
                _triggered = true;
            }
        }
    }
}
