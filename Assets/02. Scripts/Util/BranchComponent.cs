using UnityEngine.Events;

namespace Util
{
    public class BranchComponent : IFComponent
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

        private void OnFalseEvent()
        {
            FalseEvent.Invoke();
        }
    }
}