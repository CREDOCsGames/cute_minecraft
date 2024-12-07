using System;
using UnityEngine;
using Util;
using static Movement.MovementInfo;

namespace Character
{
    [RequireComponent(typeof(CharacterComponent))]
    public class DefaultStateControllerComponent : MonoBehaviour
    {
        private static readonly int STATE_IDLE = "State(Idle)".GetHashCode();
        private static readonly int STATE_WALK = "State(Walk)".GetHashCode();
        private static readonly int STATE_RUNNING = "State(Running)".GetHashCode();
        private static readonly int STATE_JUMPING = "State(Jumping)".GetHashCode();
        private static readonly int STATE_FALLING = "State(Falling)".GetHashCode();
        private static readonly int STATE_LAND = "State(Land)".GetHashCode();
        private CharacterComponent _character;

        private void ReturnBasicState()
        {
            var velY = Math.Round(_character.Rigid.velocity.y, 1);
            int actionID;

            if (_character.IsAction)
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

            _character.DoAction(actionID);
        }

        private bool IsWalked()
        {
            return (_character.Rigid.velocity.magnitude < MAX_RUN_VELOCITY);
        }

        private bool IsStopped()
        {
            return (Mathf.Abs(_character.Rigid.velocity.magnitude) < MIN_WALK_VELOCITY);
        }

        private bool IsJumpState()
        {
            return !_character.IsGrounded() && !IsStopped();
        }

        private bool IsLandState()
        {
            return _character.IsGrounded()
                   && _character.State is CharacterState.Fall;
        }

        private void Awake()
        {
            _character = GetComponent<CharacterComponent>();
            _character.IsGrounded = () => RigidbodyHelper.IsGrounded(_character.Rigid);
        }

        private void Update()
        {
            ReturnBasicState();
        }
    }
}