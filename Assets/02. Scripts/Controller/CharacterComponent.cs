using TMPro;
using UnityEngine;

namespace Controller
{
    public class LookAt
    {
        private readonly Transform _body;
        private readonly Rigidbody _rigidbody;
        public LookAt(Transform body, Rigidbody rigidbody)
        {
            _body = body;
            _rigidbody = rigidbody;
        }
        public void Update()
        {
            var lookDirection = _rigidbody.velocity;
            lookDirection.y = 0f;
            _body.LookAt(_body.position + lookDirection);
        }
    }

    public class CharacterComponent : MonoBehaviour
    {
        private LookAt _lookAt;
        public Character _character { get; private set; }
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField, Range(0, 100)] private float _moveSpeed;
        [SerializeField, Range(0, 100)] private float _jumpForce;
        public TextMeshProUGUI ui;

        public void EnterGound() => _character?.EnterGound();
        public void FinishiedAction() => _character?.FinisihedAction();

        private void Awake()
        {
            if (_animator == null || _rigidbody == null)
            {
                Debug.LogWarning($"Please check the reference: {name}");
                return;
            }
            _lookAt = new LookAt(_animator.transform, _rigidbody);
            _character = new Character(new IdleState(), _rigidbody, _animator);
            _character.MoveSpeed = _moveSpeed;
            _character.JumpForce = _jumpForce;
        }

        private void Update()
        {
            if (_animator == null || _rigidbody == null)
            {
                Debug.LogWarning($"Please check the reference: {name}");
                return;
            }
            _character.Update();
            _lookAt.Update();
            _character.MoveSpeed = _moveSpeed;
            _character.JumpForce = _jumpForce;
            ui.text = _character.State;
        }

    }
}
