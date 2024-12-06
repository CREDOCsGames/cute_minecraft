using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class ColliderEventHandler : MonoBehaviour
    {
        public UnityEvent<Collision> OnEnterEvent;
        public UnityEvent<Collision> OnExitEvent;
        public UnityEvent<Collision> OnStayEvent;
        public string TagFillter;

        private void OnCollisionEnter(Collision collision)
        {
            if (!string.IsNullOrEmpty(TagFillter) && !collision.gameObject.CompareTag(TagFillter))
            {
                return;
            }

            OnEnterEvent.Invoke(collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            if (!string.IsNullOrEmpty(TagFillter) && !collision.gameObject.CompareTag(TagFillter))
            {
                return;
            }

            OnExitEvent.Invoke(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            if (!string.IsNullOrEmpty(TagFillter) && !collision.gameObject.CompareTag(TagFillter))
            {
                return;
            }

            OnStayEvent.Invoke(collision);
        }
    }
}