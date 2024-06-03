using UnityEngine;

namespace PlatformGame.Character.Movement
{
    [CreateAssetMenu(menuName = "Custom/MovementAction/ForceMovement")]
    public class ForceMovement : MovementAction
    {
        public Vector3 Force;

        public override void PlayAction(Rigidbody rigid, MonoBehaviour coroutine)
        {
            rigid.AddForce(Force);
        }

    }
}