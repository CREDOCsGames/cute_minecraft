using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Util
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