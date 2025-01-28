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
            _timer.DoStop();
        }

        public void DoStart()
        {
            _timer.DoStart();
        }

        public void DoPause()
        {
            _timer.DoPause();
        }

        public void DoResume()
        {
            _timer.DoResume();
        }

        public void DoStop()
        {
            _timer.DoStop();
        }

        void Update()
        {
            _timer.DoTick();
        }

        void Awake()
        {
            _timer.OnPause += (t) => OnPause.Invoke(this);
            _timer.OnResume += (t) => OnResume.Invoke(this);
            _timer.OnStart += (t) => OnStart.Invoke(this);
            _timer.OnStop += (t) => OnStop.Invoke(this);
            _timer.OnTick += (t) => OnTick.Invoke(this);
            _timer.OnTimeout += (t) => OnTimeout.Invoke(this);

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