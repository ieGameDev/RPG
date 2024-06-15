using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Enemy.EnemyLoot
{
    public class LookAtPlayer : MonoBehaviour
    {
        private Transform _playerTransform;

        private void Start()
        {
            PlayerMove player = FindObjectOfType<PlayerMove>();

            if (player != null)
                _playerTransform = player.transform;
        }

        private void Update()
        {
            if (_playerTransform != null)
            {
                Vector3 direction = _playerTransform.position - transform.position;
                direction.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 3.0f * Time.deltaTime);
            }
        }
    }
}