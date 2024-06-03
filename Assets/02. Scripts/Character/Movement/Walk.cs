using UnityEngine;
using static PlatformGame.Character.Status.MovementInfo;

namespace PlatformGame.Character.Movement
{
    [CreateAssetMenu(menuName = "Custom/MovementAction/XZMovement")]
    public class Walk : MovementAction
    {
        public Vector3 mDir;

        public override void PlayAction(Rigidbody rigid, MonoBehaviour coroutine)
        {
            Debug.Assert(Camera.main, $"Main Camera is not found.");
            Debug.Assert(mDir.y == 0, $"Must have a Y value of 0. : {name}");
            var camTransform = Camera.main.transform;
            var moveForce = camTransform.right * mDir.x;
            moveForce += camTransform.forward * mDir.z;
            moveForce = moveForce.normalized * (Time.deltaTime * MOVE_SPEED);
            rigid.AddForce(moveForce);
        }

    }
}

