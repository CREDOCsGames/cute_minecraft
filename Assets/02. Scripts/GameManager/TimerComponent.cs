using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame
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
        float mElapsedTime;

        [Header("Options")]
        [SerializeField] float mTimeout;
        [SerializeField] bool mbPlayOnAwake = true;

#if DEVELOPMENT
        [Header("Debug")]
        [SerializeField] bool mbUseDebug;
        [SerializeField] string mTimeoutKey;
#endif

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
#if DEVELOPMENT
            if (mbUseDebug && !string.IsNullOrEmpty(mTimeoutKey) && UnityEngine.Input.GetKeyDown(mTimeoutKey))
            {
                DebugTimeout();
            }
#endif
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

        void Start()
        {
            if (mbPlayOnAwake)
            {
                DoStart();
            }
        }

#if DEVELOPMENT
        void DebugTimeout()
        {
            mElapsedTime = mTimeout < 5 ? mElapsedTime
                                                : mTimeout - 5f;
        }
#endif

    }
}
