using UnityEngine;


namespace PlatformGame.Character.Movement
{
    enum LookAtType
    {
        None, MoveDir, Target
    }

    public class LookAt : MonoBehaviour
    {
        Character mCharacter;
        static Vector3 m3DVelocity;
        [SerializeField] LookAtType mType;
        [SerializeField] Transform mTarget;

        public void SetTarget(Transform target)
        {
            mTarget = target;
            mCharacter = target?.GetComponent<Character>();
        }

        void LookAtMoveDir()
        {
            if (mCharacter == null)
            {
                mType = LookAtType.None;
                return;
            }

            if (mCharacter.State is not (CharacterState.Walk or CharacterState.Running))
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
            Debug.Assert(mTarget != null, $"Not found target : {name}");
            var viewPos = mTarget.position;
            viewPos.y = transform.position.y;
            transform.LookAt(viewPos);
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
                case LookAtType.MoveDir: LookAtMoveDir(); break;
                case LookAtType.Target: LookAtTarget(); break;
                default: break;
            }
        }

    }

}
