namespace Controller
{
    using UnityEngine;

    public class Character
    {
        public float MoveSpeed = 5f;
        public float JumpForce = 7f;
        public bool IsGrounded { get; private set; }
        public bool IsJumping { get; private set; } = false;
        public string State => _state?.Name;
        public bool IsFinishedAction => FinisihedAction();
        public Vector3 Position => _rigidbody.position;
        private IPlayerState _state;
        private readonly Animator _animator;
        private readonly Rigidbody _rigidbody;

        public Character(IPlayerState entryState, Rigidbody rigidbody, Animator animator)
        {
            _animator = animator;
            _rigidbody = rigidbody;
            ChangeState(entryState);
        }

        public void Update()
        {
            _state.HandleInput(this);
            _state.UpdateState(this);
        }

        public void ChangeState(IPlayerState newState)
        {
            _animator.ResetTrigger(_state?.Name ?? $"{_rigidbody.name}'s state is empty.");
            _animator.SetTrigger(newState.Name);
            _state = newState;
        }

        public void Move(Vector3 moveDirection)
        {
            _rigidbody.velocity = moveDirection * MoveSpeed;
        }

        public void Jump()
        {
            if (!IsJumping)
            {
                _rigidbody.AddForce(Vector2.up * JumpForce, ForceMode.Impulse);
                IsJumping = true;
                IsGrounded = false;
            }
        }

        public void ResetJump()
        {
            IsJumping = false;
        }

        public void EnterGound()
        {
            IsGrounded = true;
        }

        public bool FinisihedAction()
        {
            return 1f <= _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
    }
}
