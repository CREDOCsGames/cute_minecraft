using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class AnimationEventHandler : MonoBehaviour
    {
        [SerializeField] UnityEvent mOnFrameEvent;

        public void OnFrameEvent()
        {
            mOnFrameEvent.Invoke();
        }
    }
}