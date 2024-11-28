using Character;
using UnityEngine;
using static Movement.MovementInfo;

namespace Movement
{
    public class LimitSpeedComponent : MonoBehaviour
    {
        private CharacterComponent _character;
        private Rigidbody _rigid;

        private void Awake()
        {
            _character = GetComponent<CharacterComponent>();
            _rigid = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (_character == null || _character.State is //CharacterState.Walk
                    CharacterState.Run)
            {
                LimitMoveSpeed();
            }
        }

        private void LimitMoveSpeed()
        {
            var currentVelocity = _rigid.velocity;
            var currentSpeed = currentVelocity.magnitude;

            if (currentSpeed <= MAX_RUN_VELOCITY)
            {
                return;
            }

            var limitedVelocity = currentVelocity.normalized * MAX_RUN_VELOCITY;
            limitedVelocity.y = _rigid.velocity.y;
            _rigid.velocity = limitedVelocity;
        }
    }
}