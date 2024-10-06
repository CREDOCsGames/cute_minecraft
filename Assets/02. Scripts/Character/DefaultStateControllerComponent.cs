using System;
using UnityEngine;
using Util;
using static Movement.MovementInfo;

namespace Character
{
    [RequireComponent(typeof(CharacterComponent))]
    public class DefaultStateControllerComponent : MonoBehaviour
    {
        static readonly int STATE_IDLE = "State(Idle)".GetHashCode();
        static readonly int STATE_WALK = "State(Walk)".GetHashCode();
        static readonly int STATE_RUNNING = "State(Running)".GetHashCode();
        static readonly int STATE_JUMPING = "State(Jumping)".GetHashCode();
        static readonly int STATE_FALLING = "State(Falling)".GetHashCode();
        static readonly int STATE_LAND = "State(Land)".GetHashCode();
        CharacterComponent mCharacter;

        void ReturnBasicState()
        {
            var velY = Math.Round(mCharacter.Rigid.velocity.y, 1);
            int actionID;

            if (mCharacter.IsAction)
            {
                return;
            }

            else if (IsLandState())
            {
                actionID = STATE_LAND;
            }

            else if (IsJumpState())
            {
                actionID = (velY > 0) ? STATE_JUMPING : STATE_FALLING;
            }

            else
            {
                actionID = IsStopped() ? STATE_IDLE :
                    IsWalked() ? STATE_WALK :
                    STATE_RUNNING;
            }

            mCharacter.DoAction(actionID);
        }

        bool IsWalked()
        {
            return (mCharacter.Rigid.velocity.magnitude < MAX_RUN_VELOCITY);
        }

        bool IsStopped()
        {
            return (Mathf.Abs(mCharacter.Rigid.velocity.magnitude) < MIN_WALK_VELOCITY);
        }

        bool IsJumpState()
        {
            return !mCharacter.IsGrounded() && !IsStopped();
        }

        bool IsLandState()
        {
            return mCharacter.IsGrounded()
                   && mCharacter.State is CharacterState.Fall;
        }

        void Awake()
        {
            mCharacter = GetComponent<CharacterComponent>();
            mCharacter.IsGrounded = () => RigidbodyHelper.IsGrounded(mCharacter.Rigid);
        }

        void Update()
        {
            ReturnBasicState();
        }
    }
}