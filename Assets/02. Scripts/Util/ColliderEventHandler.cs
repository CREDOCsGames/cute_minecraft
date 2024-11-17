using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class ColliderEventHandler : MonoBehaviour
    {
        public UnityEvent<Collision> OnEnterEvent;
        public UnityEvent<Collision> OnExitEvent;
        public UnityEvent<Collision> OnStayEvent;
        [SerializeField] private string _tagFillter;

        private void OnCollisionEnter(Collision collision)
        {
            if (!string.IsNullOrEmpty(_tagFillter) && !collision.gameObject.CompareTag(_tagFillter))
            {
                return;
            }

            OnEnterEvent.Invoke(collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            if (!string.IsNullOrEmpty(_tagFillter) && !collision.gameObject.CompareTag(_tagFillter))
            {
                return;
            }

            OnExitEvent.Invoke(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            if (!string.IsNullOrEmpty(_tagFillter) && !collision.gameObject.CompareTag(_tagFillter))
            {
                return;
            }

            OnStayEvent.Invoke(collision);
        }
    }
}