using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class ActorUI : MonoBehaviour
    {
        [SerializeField] private HpBar _hpBar;

        private PlayerHealth _playerHealth;

        private void OnDestroy() => 
            _playerHealth.HealthChanged -= UpdateHpBar;

        public void Construct(PlayerHealth health)
        {
            _playerHealth = health;

            _playerHealth.HealthChanged += UpdateHpBar;
        }

        private void UpdateHpBar()
        {
            _hpBar.SetValue(_playerHealth.Current, _playerHealth.Max);
        }
    }
}
