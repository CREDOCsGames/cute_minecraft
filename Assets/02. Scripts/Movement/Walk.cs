using UnityEngine;
using static Movement.MovementInfo;

namespace Movement
{
    [CreateAssetMenu(menuName = "Custom/MovementAction/XZMovement")]
    public class Walk : MovementAction
    {
        [SerializeField] private Vector3 _dir;
        public bool ZeroVelocity;

        public override void PlayAction(Rigidbody rigid, MonoBehaviour coroutine)
        {
            Debug.Assert(Camera.main, $"Main Camera is not found.");
            Debug.Assert(_dir.y == 0, $"Must have a Y value of 0. : {name}");
            if (Camera.main == null)
            {
                return;
            }

            var camTransform = Camera.main.transform;
            var right = camTransform.right.x > 0 ? Vector3.right : Vector3.left;
            var forward = camTransform.forward.z > 0 ? Vector3.forward : Vector3.back;
            var moveForce = right * _dir.x;
            moveForce += forward * _dir.z;
            moveForce = moveForce.normalized * (Time.deltaTime * MOVE_SPEED);
            if (ZeroVelocity)
            {
                rigid.velocity = Vector3.zero;
            }

            rigid.AddForce(moveForce);
        }
    }
}