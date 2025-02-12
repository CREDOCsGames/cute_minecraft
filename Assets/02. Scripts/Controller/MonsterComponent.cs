using Battle;
using Unity.VisualScripting;
using UnityEngine;

namespace Controller
{
    public class MonsterComponent : MonoBehaviour
    {
        public Character _character { get; private set; }
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Animator _animator;
        [SerializeField] private AttackBoxComponent _attackBox;

        private void Awake()
        {
            _character = new Character(_rigidbody, _animator);
            _character.MoveSpeed = 1f;
            _character.OnChagedState += (s) => { if (s is CharacterState.Attack) _attackBox.OpenAttackWindow(); };
        }
        public void Attack()
        {
            _character.Attack();
        }
        public void Exit(Vector3 dir)
        {
            _character.Rigidbody.excludeLayers = -1 - LayerMask.GetMask("Wall");
            _character.ChangeController(new CanNotControl());
            _character.Rigidbody.AddForce(dir, ForceMode.Impulse);
            _character.Hit();
        }
        public void Hit()
        {
            _character?.Hit();
            _rigidbody.AddForce(-transform.forward * 5f, ForceMode.Impulse);
        }
        public void Hit(HitBoxCollision coll)
        {
            _character?.Hit();
            _character.ChangeController(new MonsterHit(_character.Controller));
            Vector3 dir = coll.Attacker.forward; dir.y = 1;
            _rigidbody.AddForce(dir * 5f, ForceMode.Impulse);
        }
        public void EnterGround()
        => _character?.EnterGound();

        private void Update()
        {
            _character?.Update();
        }
    }


    public class MonsterHit : IController
    {
        private IController _savedState;
        private readonly float _minTime;
        public MonsterHit(IController controller)
        {
            _savedState = controller;
            _minTime = Time.time + 0.5f;
        }
        public void HandleInput(Character player)
        {
            if (player.State is CharacterState.Hit && !player.IsActionFinished)
            {
                player.ExitGound();
                return;
            }

            if (Time.time < _minTime)
            {
                return;
            }

            if (player.IsGrounded)
            {
                player.ChangeController(_savedState);
                return;
            }
        }

        public void UpdateState(Character player)
        {
        }
    }
}
