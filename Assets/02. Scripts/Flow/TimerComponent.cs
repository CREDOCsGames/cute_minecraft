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

        public bool IsStart => _timer.IsStart;
        public bool IsPause => _timer.IsPause;
        public float Timeout => _timer.Timeout;
        public float ElapsedTime => _timer.ElapsedTime;
        public float LastPauseTime => _timer.LastPauseTime;

        private readonly Timer _timer = new();

        [Header("Options")][SerializeField] private float _timeout;
        [SerializeField] private bool _playOnAwake = true;

        public void Initialize(float maxTimerTim)
        {
            _timer.SetTimeout(maxTimerTim);
            _timer.Stop();
        }

        public void DoStart()
        {
            _timer.Start();
        }

        public void DoPause()
        {
            _timer.Pause();
        }

        public void DoResume()
        {
            _timer.Resume();
        }

        public void DoStop()
        {
            _timer.Stop();
        }

        void Update()
        {
            _timer.Tick();
        }

        void Awake()
        {
            _timer.OnPauseEvent += (t) => OnPause.Invoke(this);
            _timer.OnResumeEvent += (t) => OnResume.Invoke(this);
            _timer.OnStartEvent += (t) => OnStart.Invoke(this);
            _timer.OnStopEvent += (t) => OnStop.Invoke(this);
            _timer.OnTickEvent += (t) => OnTick.Invoke(this);
            _timer.OnTimeoutEvent += (t) => OnTimeout.Invoke(this);

            _timer.SetTimeout(_timeout);
        }

        void OnEnable()
        {
            if (_playOnAwake)
            {
                DoStart();
            }
        }
    }
}