using Flow;
using UnityEngine;
using UnityEngine.Events;

namespace Puzzle
{
    public class ShyBoxComponent : MonoBehaviour
    {
        private readonly Timer _tenseTimer = new();
        private readonly Timer _calmDownTimer = new();
        [SerializeField, Range(1, 100)] private int _thresholdsTime;
        [SerializeField, Range(1, 100)] private int _calmDownTime;
        [SerializeField] private UnityEvent _thresholdsEvent;
        [SerializeField] private UnityEvent<Timer> _thresholdsTickEvent;
        [SerializeField] private UnityEvent<Timer> _calmDownTickEvent;

        public void StartShowing()
        {
            if (!_tenseTimer.IsStart)
            {
                _tenseTimer.Start();
                return;
            }

            if (_calmDownTime <= _calmDownTimer.ElapsedTime)
            {
                _tenseTimer.Stop();
                _tenseTimer.Start();
            }
            else
            {
                _tenseTimer.Resume();
            }
        }

        public void HideBegin()
        {
            _tenseTimer.Pause();
            _calmDownTimer.Start();
        }

        public void HideEnd()
        {
            _calmDownTimer.Stop();
        }

        private void Awake()
        {
            _tenseTimer.SetTimeout(_thresholdsTime);
            _tenseTimer.OnTimeoutEvent += (t) => { _thresholdsEvent.Invoke(); };
            _tenseTimer.OnTickEvent += (t) => _thresholdsTickEvent.Invoke(t);

            _calmDownTimer.SetTimeout(_calmDownTime);
            _calmDownTimer.OnTickEvent += (t) => _calmDownTickEvent.Invoke(t);
        }

        private void Update()
        {
            _tenseTimer.Tick();
            _calmDownTimer.Tick();
        }
    }
}