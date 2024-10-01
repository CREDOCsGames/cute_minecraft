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
        CharacterComponent mCharacter;
        Vector3 m3DVelocity;
        [SerializeField] LookAtType mType;
        [SerializeField] Transform mTarget;

        public void SetTarget(Transform target)
        {
            mTarget = target;
            mCharacter = target?.GetComponent<CharacterComponent>();
        }

        void LookAtMoveDir()
        {
            if (mCharacter == null)
            {
                mType = LookAtType.None;
                return;
            }

            if (mCharacter.State is not (CharacterState.Walk or CharacterState.Run))
            {
                return;
            }

            var velocity = mCharacter.Rigid.velocity;

            if (Mathf.Abs(velocity.magnitude) < 1f)
            {
                return;
            }

            m3DVelocity = velocity;
            m3DVelocity.y = 0f;
            if (m3DVelocity == Vector3.zero)
            {
                return;
            }

            transform.forward = m3DVelocity;
        }

        void LookAtTarget()
        {
            if (mTarget == null)
            {
                mType = LookAtType.None;
                return;
            }

            var viewPos = mTarget.position;
            viewPos.y = transform.position.y;
            transform.LookAt(viewPos);
        }

        void LookAtMainCamera()
        {
            mTarget = Camera.main.transform;
            LookAtTarget();
        }

        void Awake()
        {
            if (mTarget == null) mTarget = null;
            SetTarget(mTarget);
        }

        void Update()
        {
            switch (mType)
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