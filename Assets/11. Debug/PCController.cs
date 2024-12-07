using System;
using System.Linq;
using Input1;
using TMPro;
using UnityEngine;

namespace Battle
{
    public class Limit
    {
        public const byte INFINITY = 0;
        private readonly byte _limit;
        private byte _count;
        public bool Able => _limit == 0 || _count < _limit;

        public Limit(byte limit)
        {
            _limit = limit;
        }

        public void Once()
        {
            _count++;
        }

        public void Reset()
        {
            _count = 0;
        }
    }

    public class LimitVelocity
    {
        private readonly Rigidbody _rigidbody;
        private readonly Vector3 _maxVelocity;

        public LimitVelocity(Rigidbody rigidbody, Vector3 maxVelocity)
        {
            _rigidbody = rigidbody;
            _maxVelocity = maxVelocity;
        }

        public void Update()
        {
            _rigidbody.velocity = new Vector3(
                Mathf.Clamp(_rigidbody.velocity.x, -_maxVelocity.x, _maxVelocity.x),
                Mathf.Clamp(_rigidbody.velocity.y, -_maxVelocity.y, _maxVelocity.y),
                Mathf.Clamp(_rigidbody.velocity.z, -_maxVelocity.z, _maxVelocity.z));
        }
    }

    public class AnimatorController
    {
        private readonly Animator _animator;
        private PCController.State _state;
        private PCController _pcController;

        public AnimatorController(Animator animator)
        {
            _animator = animator;
        }
        ~AnimatorController()
        {
            _pcController.StateChanged -= GetState;
        }
        public void SetTarget(PCController pcController)
        {
            _pcController = pcController;
            _pcController.StateChanged += GetState;
        }
        private void GetState(PCController.State newState)
        {
            if (_state != newState)
            {
                ResetTriggers(_animator);
                _animator.SetTrigger(newState.ToString());
            }

            _state = newState;
        }

        private void ResetTriggers(Animator animator)
        {
            animator.parameters
                    .Where(x => x.type == AnimatorControllerParameterType.Trigger)
                    .ToList()
                    .ForEach(t => animator.ResetTrigger(t.name));
        }
    }

    [RequireComponent(typeof(Rigidbody))]
    public class PCController : MonoBehaviour
    {
        public enum State { Idle, Walk, Run, Jump, Attack, Hit, Die }
        public State CharacterState { get; private set; }
        public event Action<State> StateChanged;
        private float _outAnimTime;
        public float JumpPower;
        public float MoveSpeed;
        private readonly Limit _jumpLimit = new(1);
        private readonly Limit _attackLimit = new(1);
        private Animator _animator;
        private Rigidbody _rigidbody;
        private LimitVelocity _limitVelocity;
        private PlayerController _inputController;
        private AnimatorController _animatorController;
        public TextMeshProUGUI ui;

        private void Awake()
        {
            _animator = transform.GetChild(0).GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
            _limitVelocity = new LimitVelocity(_rigidbody, new Vector3(3, 10, 3));
            _animatorController = new AnimatorController(_animator);
            _animatorController.SetTarget(this);
            _inputController = new PlayerController();
            _inputController.IsActive = true;

            var moveRight = new ButtonEvent();
            moveRight.InputType = InputType.Stay;
            moveRight.Key = ActionKey.Button.Right;
            moveRight.Event.AddListener(() => DoMove(Vector3.right * MoveSpeed));
            _inputController.AddButtonEvent(moveRight);

            var moveLeft = new ButtonEvent();
            moveLeft.InputType = InputType.Stay;
            moveLeft.Key = ActionKey.Button.Left;
            moveLeft.Event.AddListener(() => DoMove(Vector3.left * MoveSpeed));
            _inputController.AddButtonEvent(moveLeft);

            var moveForward = new ButtonEvent();
            moveForward.InputType = InputType.Stay;
            moveForward.Key = ActionKey.Button.Up;
            moveForward.Event.AddListener(() => DoMove(Vector3.forward * MoveSpeed));
            _inputController.AddButtonEvent(moveForward);

            var moveBack = new ButtonEvent();
            moveBack.InputType = InputType.Stay;
            moveBack.Key = ActionKey.Button.Down;
            moveBack.Event.AddListener(() => DoMove(Vector3.back * MoveSpeed));
            _inputController.AddButtonEvent(moveBack);

            var jump = new ButtonEvent();
            jump.InputType = InputType.Down;
            jump.Key = ActionKey.Button.Jump;
            jump.Event.AddListener(() => DoJump());
            _inputController.AddButtonEvent(jump);

            var attack = new ButtonEvent();
            attack.InputType = InputType.Down;
            attack.Key = ActionKey.Button.Attack;
            attack.Event.AddListener(() => DoAttack());
            _inputController.AddButtonEvent(attack);

            StateChanged += (s) => changed = true;
        }

        private bool changed;
        void Update()
        {
            changed = false;
            _limitVelocity.Update();
            _inputController.Update();
            ui.text = CharacterState.ToString();
            if (changed)
            {
                return;
            }
            DoIdle();
        }

        public void DoJump(float jumpPower = -1)
        {
            if (CharacterState != State.Jump)
            {
                _jumpLimit.Reset();
            }

            if (_jumpLimit.Able && (CharacterState is State.Idle or State.Run or State.Jump))
            {
                _rigidbody.AddForce(Vector3.up * (jumpPower == -1 ? JumpPower : jumpPower));
                CharacterState = State.Jump;
                StateChanged.Invoke(CharacterState);
                _jumpLimit.Once();
            }
        }

        public void DoIdle()
        {
            if (CharacterState is State.Jump or State.Attack)
            {
                return;
            }

            CharacterState = State.Idle;
            StateChanged.Invoke(CharacterState);
        }
        public void DoMove(Vector3 dir)
        {
            if (CharacterState is State.Attack or State.Jump)
            {
                return;
            }
            _rigidbody.AddForce(dir * MoveSpeed);
            CharacterState = State.Run;
            var lookat = _rigidbody.velocity.normalized;
            lookat.y = 0;
            transform.GetChild(0).LookAt(transform.position + lookat);
            StateChanged.Invoke(CharacterState);
        }
        public void DoAttack()
        {
            if (Time.time < _outAnimTime ||
                CharacterState is State.Jump)
            {
                return;
            }

            if (CharacterState is not State.Attack)
            {
                _attackLimit.Reset();
            }

            if (!_attackLimit.Able)
            {
                return;
            }

            CharacterState = State.Attack;
            _attackLimit.Once();
            StateChanged.Invoke(CharacterState);
        }
        public void EnterGround()
        {
            if (CharacterState is State.Jump)
            {
                CharacterState = State.Idle;
                StateChanged.Invoke(CharacterState);
            }
        }
        public void EndAttack()
        {
            if (CharacterState is State.Attack)
            {
                CharacterState = State.Idle;
                StateChanged.Invoke(CharacterState);
            }
        }

    }
}
