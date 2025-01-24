namespace Controller
{
    using System;
    using UnityEngine;
    public enum CharacterState
    {
        Idle, Jump, Run, Land, Attack, Hit, Die
    }

    public class Character
    {
        public float MoveSpeed = 5f;
        public float JumpForce = 7f;
        public bool IsGrounded { get; private set; }
        public bool IsJumping { get; private set; } = false;
        public bool IsActionFinished => _characterAnimation.IsAnimationFinished;
        public CharacterState State { get; private set; }
        public Vector3 Position => Rigidbody.position;
        public IController Controller { get; private set; }
        public readonly Rigidbody Rigidbody;
        public Transform Transform => Rigidbody.transform;
        public event Action<CharacterState> OnChagedState;
        public static bool Active { get; set; } = true;
        private readonly CharacterAnimation _characterAnimation;

        public Character(Rigidbody rigidbody, Animator animator)
        {
            _characterAnimation = new CharacterAnimation(animator);
            OnChagedState += _characterAnimation.OnChangedCharacterState;
            Rigidbody = rigidbody;
        }
        
        public void Update()
        {
            if (Controller == null|| !Active)
            {
                return;
            }
            Controller.HandleInput(this);
            Controller.UpdateState(this);
        }
        public void ChangeController(IController newController)
        {
            Controller = newController;
        }
        private void ChangeState(CharacterState newState, bool duplicateCall = false)
        {
            if (duplicateCall || State != newState)
            {
                OnChagedState?.Invoke(newState);
            }
            State = newState;
        }
        public void Move(Vector3 moveDirection)
        {
            ChangeState(CharacterState.Run);
            var velocity = moveDirection * MoveSpeed;
            Rigidbody.velocity = velocity;
        }
        public void Jump()
        {
            if (!IsJumping)
            {
                ChangeState(CharacterState.Jump, true);
                Rigidbody.AddForce(Vector2.up * JumpForce, ForceMode.Impulse);
                IsJumping = true;
                IsGrounded = false;
            }
        }
        public void Die()
        {
            ChangeState(CharacterState.Die);
        }
        public void Idle()
        {
            ChangeState(CharacterState.Idle);
            Rigidbody.velocity = Vector3.zero;
        }
        public void Hit()
        {
            ChangeState(CharacterState.Hit, true);
        }
        public void Attack()
        {
            ChangeState(CharacterState.Attack, true);
        }
        public void ResetJump()
        {
            IsJumping = false;
        }
        public void EnterGound()
        {
            IsGrounded = true;
        }
        public void ExitGound()
        {
            IsGrounded = false;
        }
        public void Land()
        {
            ChangeState(CharacterState.Land);
        }
    }
}
