using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Contents
{
    public class ShyBoxComponent : MonoBehaviour
    {
        Timer mTenseTimer = new();
        Timer mCalmDownTimer = new();
        [SerializeField, Range(1, 100)] int ThresholdsTime;
        [SerializeField, Range(1, 100)] int CalmDownTime;
        [SerializeField] UnityEvent ThresholdsEvent;
        [SerializeField] UnityEvent<Timer> ThresholdsTickEvent;
        [SerializeField] UnityEvent<Timer> CalmDownTickEvent;

        public void StartShowing()
        {
            if (!mTenseTimer.IsStart)
            {
                mTenseTimer.Start();
                return;
            }

            if (CalmDownTime <= mCalmDownTimer.ElapsedTime)
            {
                mTenseTimer.Stop();
                mTenseTimer.Start();
            }
            else
            {
                mTenseTimer.Resume();
            }
        }

        public void HideBegin()
        {
            mTenseTimer.Pause();
            mCalmDownTimer.Start();
        }

        public void HideEnd()
        {
            mCalmDownTimer.Stop();
        }

        void Awake()
        {
            mTenseTimer.SetTimeout(ThresholdsTime);
            mTenseTimer.OnTimeoutEvent += (t) => { ThresholdsEvent.Invoke(); };
            mTenseTimer.OnTickEvent += (t) => ThresholdsTickEvent.Invoke(t);

            mCalmDownTimer.SetTimeout(CalmDownTime);
            mCalmDownTimer.OnTickEvent += (t) => CalmDownTickEvent.Invoke(t);
        }

        void Update()
        {
            mTenseTimer.Tick();
            mCalmDownTimer.Tick();
        }

    }
}
