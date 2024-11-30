using System.Linq;
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

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    public class PCController : MonoBehaviour
    {
        public enum State { Idle, Walk, Run, Jump, Attack, Hit, Die }
        public float JumpPower;
        public float MoveSpeed;
        private Animator _animator;
        private Rigidbody _rb;
        private State _state;
        private float h;
        private float v;
        private bool j;
        private bool a;
        private float _outAnimTime;
        private readonly Limit _jumpLimit = new(1);
        private readonly Limit _attackLimit = new(1);

        public TextMeshProUGUI ui;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
            a = Input.GetButtonDown("Attack");
            j = Input.GetButtonDown("Jump");

            if (j)
            {
                DoJump();
            }
            else if (a)
            {
                DoAttack();
            }
            else if (h != 0 || v != 0)
            {
                DoMove(new Vector3(h, 0, v) * MoveSpeed);
            }
            else
            {
                DoIdle();
                //ControlAnimator();
            }
            ui.text = _state.ToString();
        }

        private void ControlAnimator()
        {
            _animator.parameters
                .Where(x => x.type == AnimatorControllerParameterType.Trigger)
                .ToList()
                .ForEach(t => _animator.ResetTrigger(t.name));
            _animator.SetTrigger(_state.ToString());
            _animator.Update(Time.deltaTime);
        }

        private void DoJump()
        {
            if (_state != State.Jump)
            {
                _jumpLimit.Reset();
            }

            if (_jumpLimit.Able)
            {
                _rb.AddForce(Vector3.up * JumpPower);
                _state = State.Jump;
                _jumpLimit.Once();
                ControlAnimator();
            }
        }

        private void DoIdle()
        {
            if (_state is State.Attack && _outAnimTime < Time.time ||
                _state is State.Walk or State.Run)
            {
                _state = State.Idle;
                ControlAnimator();
            }
        }

        private void DoMove(Vector3 dir)
        {
            if (_state is State.Attack or State.Jump)
            {
                return;
            }
            _rb.AddForce(dir * MoveSpeed);
            _state = State.Run;
            ControlAnimator();
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
            ControlAnimator();
            _attackLimit.Once();
            _outAnimTime = Time.time + _animator.GetNextAnimatorStateInfo(0).length;
        }

        public void EnterGround()
        {
            if (_state is State.Jump)
            {
                _state = State.Idle;
                ControlAnimator();
            }
        }
    }

}
