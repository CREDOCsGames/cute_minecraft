using System.Linq;
using UnityEngine;

namespace Util
{
    public class RigidBodyHandler : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigid;

        public Rigidbody Rigid
        {
            get
            {
                Debug.Assert(_rigid != null);
                return _rigid;
            }
        }

        public void AddForce(float power)
        {
            Rigid.AddForce(transform.forward * power);
        }

        public void ResetVelocity()
        {
            Rigid.velocity = Vector3.zero;
        }

        public void SetKinematic(bool kinematic)
        {
            Rigid.isKinematic = kinematic;
        }
    }
}