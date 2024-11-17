using Character;
using UnityEngine;


namespace Util
{
    enum LookAtType
    {
        None,
        MoveDir,
        Target,
        MainCamera
    }

    public class LookAtComponent : MonoBehaviour
    {
        CharacterComponent _character;
        Vector3 _3DVelocity;
        [SerializeField] LookAtType _type;
        [SerializeField] Transform _target;

        public void SetTarget(Transform target)
        {
            _target = target;
            _character = target?.GetComponent<CharacterComponent>();
        }

        private void LookAtMoveDir()
        {
            if (_character == null)
            {
                _type = LookAtType.None;
                return;
            }

            if (_character.State is not (CharacterState.Walk or CharacterState.Run))
            {
                return;
            }

            var velocity = _character.Rigid.velocity;

            if (Mathf.Abs(velocity.magnitude) < 1f)
            {
                return;
            }

            _3DVelocity = velocity;
            _3DVelocity.y = 0f;
            if (_3DVelocity == Vector3.zero)
            {
                return;
            }

            transform.forward = _3DVelocity;
        }

        private void LookAtTarget()
        {
            if (_target == null)
            {
                _type = LookAtType.None;
                return;
            }

            var viewPos = _target.position;
            viewPos.y = transform.position.y;
            transform.LookAt(viewPos);
        }

        private void LookAtMainCamera()
        {
            _target = Camera.main.transform;
            LookAtTarget();
        }

        private void Awake()
        {
            if (_target == null) _target = null;
            SetTarget(_target);
        }

        private void Update()
        {
            switch (_type)
            {
                case LookAtType.None: break;
                case LookAtType.MoveDir:
                    LookAtMoveDir();
                    break;
                case LookAtType.Target:
                    LookAtTarget();
                    break;
                case LookAtType.MainCamera:
                    LookAtMainCamera();
                    break;
                default: break;
            }
        }
    }
}