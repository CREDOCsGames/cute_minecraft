using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class MouseEventHandler : MonoBehaviour
    {
        public UnityEvent MouseDownEvent;
        public UnityEvent MouseUpEvent;

        void OnMouseDown()
        {
            MouseDownEvent.Invoke();
        }

        void OnMouseUp()
        {
            MouseUpEvent.Invoke();
        }
    }
}