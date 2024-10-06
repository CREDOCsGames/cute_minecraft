using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class ColliderEventHandler : MonoBehaviour
    {
        public UnityEvent<Collision> OnEnterEvent;
        public UnityEvent<Collision> OnExitEvent;
        public UnityEvent<Collision> OnStayEvent;
        [SerializeField] string TagFillter;

        void OnCollisionEnter(Collision collision)
        {
            if (!string.IsNullOrEmpty(TagFillter) && !collision.gameObject.CompareTag(TagFillter))
            {
                return;
            }

            OnEnterEvent.Invoke(collision);
        }

        void OnCollisionExit(Collision collision)
        {
            if (!string.IsNullOrEmpty(TagFillter) && !collision.gameObject.CompareTag(TagFillter))
            {
                return;
            }

            OnExitEvent.Invoke(collision);
        }

        void OnCollisionStay(Collision collision)
        {
            if (!string.IsNullOrEmpty(TagFillter) && !collision.gameObject.CompareTag(TagFillter))
            {
                return;
            }

            OnStayEvent.Invoke(collision);
        }
    }
}