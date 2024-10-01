using System;

namespace Flow
{
    public class Movie
    {
        public event Action OnPlayEvent;
        public event Action OnEndEvent;
        public event Action OnSkipEvent;

        public void OnPlay()
        {
            OnPlayEvent?.Invoke();
        }

        public void OnEnd()
        {
            OnEndEvent?.Invoke();
        }

        public void OnSkip()
        {
            OnSkipEvent?.Invoke();
        }
    }
}