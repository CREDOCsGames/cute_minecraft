using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class EnableEventHandler : MonoBehaviour
    {
        public UnityEvent OnEnableEvent;
        public UnityEvent OnDisableEvent;

        public void OnEnable()
        {
            OnEnableEvent.Invoke();
        }

        private void OnDisable()
        {
            OnDisableEvent.Invoke();
        }
    }
}