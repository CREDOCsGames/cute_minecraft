using UnityEngine;

namespace Movement
{
    [CreateAssetMenu(menuName = "Custom/MovementAction/ForceMovement")]
    public class ForceMovement : MovementAction
    {
        [SerializeField] private Vector3 _force;
        [SerializeField] private bool _useZeroVelocity;

        public override void PlayAction(Rigidbody rigid, MonoBehaviour coroutine)
        {
            if (_useZeroVelocity)
            {
                rigid.velocity = Vector3.zero;
            }

            rigid.AddForce(_force);
        }
    }
}