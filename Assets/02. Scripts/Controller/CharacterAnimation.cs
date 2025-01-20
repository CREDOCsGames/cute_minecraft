using UnityEngine;

namespace Controller
{
    public class CharacterAnimation
    {
        public bool IsAnimationFinished => (1f <= _animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        private readonly Animator _animator;
        private CharacterState _currentState;
        public CharacterAnimation(Animator animator)
        {
            _animator = animator;
        }
        public void OnChangedCharacterState(CharacterState newState)
        {
            TransitionIntoNextState(newState);
        }
        private void TransitionIntoNextState(CharacterState newState)
        {
            _animator.ResetTrigger(_currentState.ToString());
            _animator.SetTrigger(newState.ToString());
            _currentState = newState;
        }
    }
}