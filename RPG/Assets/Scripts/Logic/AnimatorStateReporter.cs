using UnityEngine;

namespace Assets.Scripts.Logic
{
    public class AnimatorStateReporter : StateMachineBehaviour
    {
        private IAnimationStateReader _stateReader;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            Findreader(animator);

            _stateReader.EnteredState(stateInfo.shortNameHash);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            Findreader(animator);

            _stateReader.ExitedState(stateInfo.shortNameHash);
        }

        private void Findreader(Animator animator)
        {
            if (_stateReader != null)
                return;

            _stateReader = animator.gameObject.GetComponent<IAnimationStateReader>();
        }
    }
}
