using UnityEngine;

namespace Battle
{
    public class KnockBack : MonoBehaviour
    {
        [SerializeField] private float _power;
        [SerializeField] private Rigidbody _rigidbidy;

        public void OnHit(HitBoxCollision coll)
        {
            if (_rigidbidy != null)
            {
                _rigidbidy.AddForce(coll.Attacker.transform.forward * _power, ForceMode.Impulse);
            }
        }

    }
}
