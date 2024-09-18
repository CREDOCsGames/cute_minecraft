using System;

namespace PlatformGame.Manager
{
    public class Selection
    {
        public event Action OnEnterEvent;
        public event Action OnSelectEvent;
        public event Action OnChangeEvent;

        public void OnEnter()
        {
            OnEnterEvent?.Invoke();
        }

        public void OnChange()
        {
            OnChangeEvent?.Invoke();
        }

        public void OnSelect()
        {
            OnSelectEvent?.Invoke();
        }

        public void Skip()
        {
            OnSelect();
        }
    }

}
