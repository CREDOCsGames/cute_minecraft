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
                Mathf.Min(_rigidbody.velocity.x, _maxVelocity.x),
                Mathf.Min(_rigidbody.velocity.y, _maxVelocity.y),
                Mathf.Min(_rigidbody.velocity.z, _maxVelocity.z));
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
                Debug.Log(_animator.GetNextAnimatorStateInfo(0).length);
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
        private State _state;
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
            ui.text = _state.ToString();
            if (0.5f < _inputController.CoolingDuration)
            {
                DoIdle();
            }
        }

        private void DoJump()
        {
            if (_state != State.Jump)
            {
                _jumpLimit.Reset();
            }

            if (_jumpLimit.Able && _outAnimTime < Time.time)
            {
                _rb.AddForce(Vector3.up * JumpPower);
                _state = State.Jump;
                _jumpLimit.Once();
                _outAnimTime = Time.time + _animator.GetNextAnimatorStateInfo(0).length;
                StateChanged.Invoke(_state);
            }
        }

        private void DoIdle()
        {
            if (_state is State.Jump)
            {
                return;
            }

            if (_state is State.Attack && _outAnimTime < Time.time ||
                _state is State.Walk or State.Run)
            {
                _state = State.Idle;
                StateChanged.Invoke(_state);
            }
        }

        private void DoMove(Vector3 dir)
        {
            if ((_state == State.Attack && Time.time < _outAnimTime) || _state == State.Jump)
            {
                return;
            }
            _rb.AddForce(dir * MoveSpeed);
            _state = State.Run;
            transform.GetChild(0).LookAt(transform.position + dir);
            StateChanged.Invoke(_state);
        }

        private void DoAttack()
        {
            if (Time.time < _outAnimTime ||
                _state is State.Jump)
            {
                return;
            }

            if (_state is not State.Attack)
            {
                _attackLimit.Reset();
            }

            if (!_attackLimit.Able)
            {
                return;
            }

            _state = State.Attack;
            _attackLimit.Once();
            _outAnimTime = Time.time + _animator.GetNextAnimatorStateInfo(0).length;
            StateChanged.Invoke(_state);
        }

        public void EnterGround()
        {
            if (_state is State.Jump)
            {
                _state = State.Idle;
                StateChanged.Invoke(_state);
            }
        }
    }

}
