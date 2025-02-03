using System;
using System.Collections;

namespace Flow
{
    public class Timer
    {
        public event Action<Timer> OnStart;
        public event Action<Timer> OnStop;
        public event Action<Timer> OnPause;
        public event Action<Timer> OnResume;
        public event Action<Timer> OnTimeout;
        public event Action<Timer> OnTick;
        public bool IsPause { get; private set; }
        public bool IsStart { get; private set; }
        public float Timeout { get; private set; }
        public float ElapsedTime { get; private set; }
        public float LastPauseTime { get; private set; }
        private float _lastTickTime { get; set; }
        private static float _serverTime => ServerTime.Time;

        public Timer()
        {
        }
        public void DoStart()
        {
            if (IsStart)
            {
                return;
            }

            IsStart = true;
            IsPause = false;
            ElapsedTime = 0f;
            _lastTickTime = _serverTime;
            OnStart?.Invoke(this);
        }
        public void DoStop()
        {
            if (IsStart == false)
            {
                return;
            }

            IsStart = false;
            OnStop?.Invoke(this);
        }
        public void DoPause()
        {
            if (IsPause)
            {
                return;
            }

            IsPause = true;
            LastPauseTime = _serverTime;
            OnPause?.Invoke(this);
        }
        public void DoResume()
        {
            if (!IsPause)
            {
                return;
            }

            IsPause = false;
            OnResume?.Invoke(this);
            _lastTickTime = _serverTime;
        }
        public void SetTimeout(float timeout)
        {
            Timeout = timeout;
        }
        public void RemoveTimeout()
        {
            Timeout = 0f;
        }
        public void DoTick()
        {
            if (!IsStart || IsPause)
            {
                return;
            }

            ElapsedTime += _serverTime - _lastTickTime;
            _lastTickTime = _serverTime;
            OnTick?.Invoke(this);

            if (Timeout == 0)
            {
                return;
            }

            if (Timeout < ElapsedTime)
            {
                DoTimeout();
            }
        }
        public IEnumerator Update()
        {
            while (true)
            {
                yield return null;
                DoTick();
            }
        }
        private void DoTimeout()
        {
            IsStart = false;
            OnTimeout?.Invoke(this);
        }
    }
}