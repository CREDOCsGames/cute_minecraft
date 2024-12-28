using Battle;
using UnityEngine;

namespace Controller
{
    public class MonsterComponent : MonoBehaviour
    {
        public Character _character { get; private set; }
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Animator _animator;

        private void Awake()
        {
            _character = new Character(_rigidbody, _animator);
            _character.MoveSpeed = 1f;
        }
        public void Attack()
        {
            _character.Attack();
        }
        public void Exit(Vector3 dir)
        {
            _character.Rigidbody.excludeLayers = -1;
            _character.ChangeController(new CanNotControl());
            _character.Rigidbody.AddForce(dir * 17f, ForceMode.Impulse);
        }
        public void Hit()
        {
            _character?.Hit();
            _rigidbody.AddForce(-transform.forward * 5f, ForceMode.Impulse);
        }
        public void Hit(HitBoxCollision coll)
        {
            _character?.Hit();
            _character.ChangeController(new MonsterHit());
            Vector3 dir = coll.Attacker.forward.normalized; dir.y = 1;
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
        public void HandleInput(Character player)
        {
            if (player.State is CharacterState.Hit && !player.IsFinishedAction)
            {
                player.ExitGound();
                return;
            }

            if (player.IsGrounded)
            {
                player.ChangeController(new MonsterState());
                return;
            }
        }

        public void UpdateState(Character player)
        {
        }
    }
}
