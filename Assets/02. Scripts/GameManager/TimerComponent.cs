using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame
{

    public class TimerComponent : MonoBehaviour
    {
        public UnityEvent OnStartTimer;
        public UnityEvent OnStopTimer;
        public UnityEvent OnPauseTimer;
        public UnityEvent OnResumeTimer;
        public UnityEvent OnTick;
        public UnityEvent OnTimeout;

        public bool IsStart => mTimer.IsStart;
        public bool IsPause => mTimer.IsPause;
        public float Timeout => mTimer.Timeout;
        public float ElapsedTime => mTimer.ElapsedTime;
        public float LastPauseTime => mTimer.LastPauseTime;

        Timer mTimer = new();
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

        public void StartTimer()
        {
            mTimer.Start();
        }

        public void PauseTimer()
        {
            mTimer.Pause();
        }

        public void ResumeTimer()
        {
            mTimer.Resume();
        }

        public void StopTimer()
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
            mTimer.OnPauseEvent += (t) => OnPauseTimer.Invoke();
            mTimer.OnResumeEvent += (t) => OnResumeTimer.Invoke();
            mTimer.OnStartEvent += (t) => OnStartTimer.Invoke();
            mTimer.OnStopEvent += (t) => OnStopTimer.Invoke();
            mTimer.OnTickEvent += (t) => OnTick.Invoke();
            mTimer.OnTimeoutEvent += (t) => OnTimeout.Invoke();

            mTimer.SetTimeout(mTimeout);
        }

        void Start()
        {
            if (mbPlayOnAwake)
            {
                StartTimer();
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
