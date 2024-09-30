
using UnityEngine.Events;

namespace PlatformGame.Util
{
    public class BranchComponent : IF
    {
        public UnityEvent FalseEvent;

        protected override bool InvokeConditions()
        {
            if (base.InvokeConditions())
            {
                return true;
            }
            OnFalseEvent();
            return false;
        }

        public void AddListenerFalseEvent(UnityAction listener)
        {
            FalseEvent.AddListener(listener);
        }

        public void RemoveListenerFalseEvent(UnityAction listener)
        {
            FalseEvent.RemoveListener(listener);
        }

        void OnFalseEvent()
        {
            FalseEvent.Invoke();
        }
    }

}
