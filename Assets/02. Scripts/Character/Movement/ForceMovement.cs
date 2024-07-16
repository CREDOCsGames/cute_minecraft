using UnityEngine;

namespace PlatformGame.Character.Movement
{
    [CreateAssetMenu(menuName = "Custom/MovementAction/ForceMovement")]
    public class ForceMovement : MovementAction
    {
        public Vector3 Force;
        public bool ZeroVelocity;
        public override void PlayAction(Rigidbody rigid, MonoBehaviour coroutine)
        {
            if (ZeroVelocity)
            {
                rigid.velocity = Vector3.zero;
            }
            rigid.AddForce(Force);
        }

    }
}