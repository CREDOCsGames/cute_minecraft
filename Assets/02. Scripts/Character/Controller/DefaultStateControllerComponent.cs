using PlatformGame.Util;
using System;
using UnityEngine;
using static PlatformGame.Character.Status.MovementInfo;

namespace PlatformGame.Character.Controller
{
    [RequireComponent(typeof(CharacterComponent))]
    public class DefaultStateControllerComponent : MonoBehaviour
    {
        int STATE_IDLE => "State(Idle)".GetHashCode();
        int STATE_WALK = "State(Walk)".GetHashCode();
        int STATE_RUNNING = "State(Running)".GetHashCode();
        int STATE_JUMPING = "State(Jumping)".GetHashCode();
        int STATE_FALLING = "State(Falling)".GetHashCode();
        int STATE_LAND = "State(Land)".GetHashCode();
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