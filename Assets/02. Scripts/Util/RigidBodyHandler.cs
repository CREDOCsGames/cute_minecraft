using Character;
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

        public void Explosion(float force)
        {
            CharacterComponent.Instances.Where(x =>
                    Vector3.Distance(x.transform.position, _rigid.transform.position) < force * 0.1f)
                .ToList()
                .ForEach(x => x.Rigid.AddExplosionForce(force, transform.position, force * 0.1f, force * 0.5f));
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