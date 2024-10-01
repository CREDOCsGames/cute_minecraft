using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class TriggerEventHandler : MonoBehaviour
    {
        public UnityEvent<Collider> mOnCollisionEnter;
        public UnityEvent<Collider> mOnCollisionExit;
        public UnityEvent<Collider> mOnCollisionStay;

        void OnTriggerEnter(Collider coll)
        {
            mOnCollisionEnter.Invoke(coll);
        }

        void OnTriggerExit(Collider coll)
        {
            mOnCollisionExit.Invoke(coll);
        }

        void OnTriggerStay(Collider coll)
        {
            mOnCollisionStay.Invoke(coll);
        }
    }
}