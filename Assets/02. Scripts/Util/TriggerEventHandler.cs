using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class TriggerEventHandler : MonoBehaviour
    {
        public UnityEvent<Collider> _onCollisionEnter;
        public UnityEvent<Collider> _onCollisionExit;
        public UnityEvent<Collider> _onCollisionStay;

        private void OnTriggerEnter(Collider coll)
        {
            _onCollisionEnter.Invoke(coll);
        }

        private void OnTriggerExit(Collider coll)
        {
            _onCollisionExit.Invoke(coll);
        }

        private void OnTriggerStay(Collider coll)
        {
            _onCollisionStay.Invoke(coll);
        }
    }
}