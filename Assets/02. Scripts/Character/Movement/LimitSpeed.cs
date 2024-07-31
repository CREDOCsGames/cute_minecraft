using UnityEngine;
using static PlatformGame.Character.Status.MovementInfo;

namespace PlatformGame.Character.Movement
{
    [RequireComponent(typeof(Character))]
    public class LimitSpeed : MonoBehaviour
    {
        Character mCharacter;

        void Awake()
        {
            mCharacter = GetComponent<Character>();
        }

        void FixedUpdate()
        {
            if (mCharacter.State is CharacterState.Walk
                                or CharacterState.Run)
            {
                LimitMoveSpeed();
            }
        }

        void LimitMoveSpeed()
        {
            var currentVelocity = mCharacter.Rigid.velocity;
            var currentSpeed = currentVelocity.magnitude;

            if (currentSpeed <= MAX_RUN_VELOCITY)
            {
                return;
            }

            var limitedVelocity = currentVelocity.normalized * MAX_RUN_VELOCITY;
            limitedVelocity.y = mCharacter.Rigid.velocity.y;
            mCharacter.Rigid.velocity = limitedVelocity;
        }
    }
}