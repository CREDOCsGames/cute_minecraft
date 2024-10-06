using UnityEngine;

namespace RootMotion
{
    public class LookAtAround : MonoBehaviour
    {
        [SerializeField] Rigidbody Rigid;
        [SerializeField] bool FixedY;

        void Update()
        {
            var velocity = Rigid.velocity;
            var pos = velocity.normalized;

            if (FixedY)
            {
                pos.y = transform.localPosition.y;
            }

            if (pos.x == 0 && pos.z == 0)
            {
                return;
            }
            transform.localPosition = pos;
        }
    }

}
