using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        private static readonly int MoveHash = Animator.StringToHash("Walking");

        [SerializeField] private Animator _animator;
        [SerializeField] private CharacterController _characterController;

        private void Update()
        {
            _animator.SetFloat(MoveHash, _characterController.velocity.magnitude, 0.1f, Time.deltaTime);
        }
    }
}
