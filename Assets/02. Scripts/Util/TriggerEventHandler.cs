using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Util
{
    public class TriggerEventHandler : MonoBehaviour
    {
        public UnityEvent mOnCollisionEnter;
        public UnityEvent mOnCollisionExit;
        public UnityEvent mOnCollisionStay;

        void OnTriggerEnter(Collider coll)
        {
            mOnCollisionEnter.Invoke();
        }

        void OnTriggerExit(Collider coll)
        {
            mOnCollisionExit.Invoke();
        }

        void OnTriggerStay(Collider coll)
        {
            mOnCollisionStay.Invoke();
        }
    }

}