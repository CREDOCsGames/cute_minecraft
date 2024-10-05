using System;
using UnityEngine;

namespace PlatformGame
{
    public class Timer
    {
        public event Action<Timer> OnStartEvent;
        public event Action<Timer> OnStopEvent;
        public event Action<Timer> OnPauseEvent;
        public event Action<Timer> OnResumeEvent;
        public event Action<Timer> OnTimeoutEvent;
        public event Action<Timer> OnTickEvent;

        public bool IsPause { get; private set; }

        public bool IsStart { get; private set; }

        public float Timeout { get; private set; }

        public float ElapsedTime { get; private set; }

        public float LastPauseTime { get; private set; }

        private float LastTickTime { get; set; }

        static float ServerTime => Server.ServerTime.Time;

        public Timer() { }
        public Timer(float timeout, Action<Timer> timeOutAction)
        {
            SetTimeout(timeout);
            OnTimeoutEvent += timeOutAction;
        }

        public void Start()
        {
            if (IsStart)
            {
                return;
            }

            IsStart = true;
            IsPause = false;
            ElapsedTime = 0f;
            LastTickTime = ServerTime;
            OnStartEvent?.Invoke(this);
        }

        public void Stop()
        {
            if (IsStart == false)
            {
                return;
            }

            IsStart = false;
            OnStopEvent?.Invoke(this);
        }

        public void Pause()
        {
            if (IsPause)
            {
                return;
            }

            IsPause = true;
            LastPauseTime = ServerTime;
            OnPauseEvent?.Invoke(this);
        }

        public void Resume()
        {
            if (!IsPause)
            {
                return;
            }

            IsPause = false;
            OnResumeEvent?.Invoke(this);
            LastTickTime = ServerTime;
        }

        public void SetTimeout(float timeout)
        {
            Timeout = timeout;
        }

        public void RemoveTimeout()
        {
            Timeout = 0f;
        }

        public void Tick()
        {
            if (!IsStart || IsPause)
            {
                return;
            }

            ElapsedTime += ServerTime - LastTickTime;
            LastTickTime = ServerTime;
            OnTickEvent?.Invoke(this);

            if (Timeout == 0)
            {
                return;
            }

            if (Timeout < ElapsedTime)
            {
                DoTimeout();
            }
        }

        void DoTimeout()
        {
            IsStart = false;
            OnTimeoutEvent?.Invoke(this);
        }

    }
}