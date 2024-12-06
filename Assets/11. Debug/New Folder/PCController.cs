using System;
using System.Linq;
using Input1;
using TMPro;
using UnityEngine;

namespace A
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
        private Animator _animator;
        private int _postClip;
        private PCController.State _state;
        public AnimatorController(Animator animator)
        {
            _animator = animator;
        }

        public void Update()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).shortNameHash != _postClip)
            {
                _animator.SetTrigger(_state.ToString());
                _animator.Update(Time.deltaTime);
            }
            else
            {
                _animator.parameters
                        .Where(x => x.type == AnimatorControllerParameterType.Trigger)
                        .ToList()
                        .ForEach(t => _animator.ResetTrigger(t.name));
            }
        }

        private void GetState(PCController.State newState)
        {
            if (_state != newState)
            {
                _animator.parameters
                        .Where(x => x.type == AnimatorControllerParameterType.Trigger)
                        .ToList()
                        .ForEach(t => _animator.ResetTrigger(t.name));

                _animator.SetTrigger(newState.ToString());
                _animator.Update(Time.deltaTime);
                _postClip = _animator.GetNextAnimatorStateInfo(0).shortNameHash;
            }

            _state = newState;
        }

        public void SetTarget(PCController pCController)
        {
            pCController.StateChanged += GetState;
        }
    }


    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    public class PCController : MonoBehaviour
    {
        public AnimationClip A;

        public enum State { Idle, Walk, Run, Jump, Attack, Hit, Die }
        public float JumpPower;
        public float MoveSpeed;
        private Animator _animator;
        private Rigidbody _rb;
        public State CharacterState { get; private set; }
        private float _outAnimTime;
        private readonly Limit _jumpLimit = new(1);
        private readonly Limit _attackLimit = new(1);
        private LimitVelocity _limitVelocity;
        public TextMeshProUGUI ui;
        public event Action<State> StateChanged;
        private AnimatorController _animatorController;
        private PlayerController _inputController;

        private void Awake()
        {
            _animator = transform.GetChild(0).GetComponent<Animator>();
            _rb = GetComponent<Rigidbody>();
            _limitVelocity = new LimitVelocity(_rb, new Vector3(3, 10, 3));
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
        }

        void Update()
        {

            _limitVelocity.Update();
            _animatorController.Update();
            _inputController.Update();
            ui.text = CharacterState.ToString();
            if (0.5f < _inputController.CoolingDuration)
            {
                DoIdle();
            }
        }

        public void DoJump(float jumpPower = -1)
        {
            if (CharacterState != State.Jump)
            {
                _jumpLimit.Reset();
            }

            if (_jumpLimit.Able && _outAnimTime < Time.time)
            {
                _rb.AddForce(Vector3.up * (jumpPower == -1 ? JumpPower : jumpPower));
                CharacterState = State.Jump;
                _jumpLimit.Once();
                _outAnimTime = Time.time + _animator.GetNextAnimatorStateInfo(0).length;
                StateChanged.Invoke(CharacterState);
            }
        }

        private void DoIdle()
        {
            if (CharacterState is State.Jump)
            {
                return;
            }

            if (CharacterState is State.Attack && _outAnimTime < Time.time ||
                CharacterState is State.Walk or State.Run)
            {
                CharacterState = State.Idle;
                StateChanged.Invoke(CharacterState);
            }
        }

        public void DoMove(Vector3 dir)
        {
            if ((CharacterState == State.Attack && Time.time < _outAnimTime) || CharacterState == State.Jump)
            {
                return;
            }
            _rb.AddForce(dir * MoveSpeed);
            CharacterState = State.Run;
            var lookat = _rb.velocity.normalized;
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
            _outAnimTime = Time.time + _animator.GetNextAnimatorStateInfo(0).length;
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
    }

}
