using Character;
using UnityEngine;
using static Movement.MovementInfo;

namespace Movement
{
    public class LimitSpeedComponent : MonoBehaviour
    {
        CharacterComponent mCharacter;
        Rigidbody mRigid;

        void Awake()
        {
            mCharacter = GetComponent<CharacterComponent>();
            mRigid = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            if (mCharacter == null || mCharacter.State is CharacterState.Walk
                    or CharacterState.Run)
            {
                LimitMoveSpeed();
            }
        }

        void LimitMoveSpeed()
        {
            var currentVelocity = mRigid.velocity;
            var currentSpeed = currentVelocity.magnitude;

            if (currentSpeed <= MAX_RUN_VELOCITY)
            {
                return;
            }

            var limitedVelocity = currentVelocity.normalized * MAX_RUN_VELOCITY;
            limitedVelocity.y = mRigid.velocity.y;
            mRigid.velocity = limitedVelocity;
        }
    }
}