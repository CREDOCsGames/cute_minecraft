using UnityEngine;
using UnityEngine.Events;

namespace Flow
{
    public class TimerComponent : MonoBehaviour
    {
        public UnityEvent<TimerComponent> OnStart;
        public UnityEvent<TimerComponent> OnStop;
        public UnityEvent<TimerComponent> OnPause;
        public UnityEvent<TimerComponent> OnResume;
        public UnityEvent<TimerComponent> OnTick;
        public UnityEvent<TimerComponent> OnTimeout;

        public bool IsStart => mTimer.IsStart;
        public bool IsPause => mTimer.IsPause;
        public float Timeout => mTimer.Timeout;
        public float ElapsedTime => mTimer.ElapsedTime;
        public float LastPauseTime => mTimer.LastPauseTime;

        readonly Timer mTimer = new();

        [Header("Options")] [SerializeField] float mTimeout;
        [SerializeField] bool mbPlayOnAwake = true;

        public void Initialize(float maxTimerTim)
        {
            mTimer.SetTimeout(maxTimerTim);
            mTimer.Stop();
        }

        public void DoStart()
        {
            mTimer.Start();
        }

        public void DoPause()
        {
            mTimer.Pause();
        }

        public void DoResume()
        {
            mTimer.Resume();
        }

        public void DoStop()
        {
            mTimer.Stop();
        }

        void Update()
        {
            mTimer.Tick();
        }

        void Awake()
        {
            mTimer.OnPauseEvent += (t) => OnPause.Invoke(this);
            mTimer.OnResumeEvent += (t) => OnResume.Invoke(this);
            mTimer.OnStartEvent += (t) => OnStart.Invoke(this);
            mTimer.OnStopEvent += (t) => OnStop.Invoke(this);
            mTimer.OnTickEvent += (t) => OnTick.Invoke(this);
            mTimer.OnTimeoutEvent += (t) => OnTimeout.Invoke(this);

            mTimer.SetTimeout(mTimeout);
        }

        void OnEnable()
        {
            if (mbPlayOnAwake)
            {
                DoStart();
            }
        }
    }
}