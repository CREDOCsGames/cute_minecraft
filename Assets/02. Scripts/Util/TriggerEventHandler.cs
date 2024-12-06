using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class TriggerEventHandler : MonoBehaviour
    {
        public UnityEvent<Collider> _onCollisionEnter;
        public UnityEvent<Collider> _onCollisionExit;
        public UnityEvent<Collider> _onCollisionStay;
        public string Filter;

        private void OnTriggerEnter(Collider coll)
        {
            if (!string.IsNullOrEmpty(Filter) && !coll.gameObject.CompareTag(Filter))
            {
                return;
            }
            _onCollisionEnter.Invoke(coll);
        }

        private void OnTriggerExit(Collider coll)
        {
            if (!string.IsNullOrEmpty(Filter) && !coll.gameObject.CompareTag(Filter))
            {
                return;
            }
            _onCollisionExit.Invoke(coll);
        }

        private void OnTriggerStay(Collider coll)
        {
            if (!string.IsNullOrEmpty(Filter) && !coll.gameObject.CompareTag(Filter))
            {
                return;
            }
            _onCollisionStay.Invoke(coll);
        }
    }
}