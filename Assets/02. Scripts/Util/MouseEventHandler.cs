using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class MouseEventHandler : MonoBehaviour
    {
        public UnityEvent MouseDownEvent;
        public UnityEvent MouseUpEvent;

        private void OnMouseDown()
        {
            MouseDownEvent.Invoke();
        }

        private void OnMouseUp()
        {
            MouseUpEvent.Invoke();
        }
    }
}