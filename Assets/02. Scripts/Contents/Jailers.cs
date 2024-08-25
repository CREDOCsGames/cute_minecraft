using PlatformGame.Character.Collision;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Contents
{
    public class Jailers : MonoBehaviour
    {
        Timer mTimer = new();

        [Header("[Step1. Observe]")]
        [SerializeField] UnityEvent EnterFieldOfView;
        [SerializeField] UnityEvent LeaveFieldOfView;

        [Header("[Step2. Recognition]")]
        [SerializeField, Range(1, 100)] float TotalRecognitionTime;
        [SerializeField] UnityEvent EnterAlertLevel1;
        [SerializeField] UnityEvent EnterAlertLevel2;
        [SerializeField] UnityEvent EnterAlertLevel3;

        [Header("[Step4. Action]")]
        [SerializeField] UnityEvent DoAction;

        void OnAlertLevel1(Timer t = null)
        {
            EnterAlertLevel1.Invoke();
            mTimer.Start();
        }

        void OnAlertLevel2(Timer t = null)
        {
            EnterAlertLevel2.Invoke();
            mTimer.OnTimeoutEvent -= OnAlertLevel2;
            mTimer.OnTimeoutEvent += OnAlertLevel3;
            mTimer.Start();
        }

        void OnAlertLevel3(Timer t = null)
        {
            EnterAlertLevel3.Invoke();
            mTimer.OnTimeoutEvent -= OnAlertLevel3;
            mTimer.OnTimeoutEvent += OnAction;
            mTimer.Start();
        }

        void OnEnterFieldOfView()
        {
            EnterFieldOfView.Invoke();
            mTimer.OnTimeoutEvent += OnAlertLevel2;
            OnAlertLevel1();
        }

        void OnLeaveFieldOfView()
        {
            mTimer.OnTimeoutEvent -= OnAlertLevel1;
            mTimer.OnTimeoutEvent -= OnAlertLevel2;
            mTimer.OnTimeoutEvent -= OnAlertLevel3;
            mTimer.OnTimeoutEvent -= OnAction;
            mTimer.Stop();
            LeaveFieldOfView.Invoke();
        }

        void OnAction(Timer t)
        {
            DoAction.Invoke();
        }

        void Awake()
        {
            mTimer.SetTimeout(TotalRecognitionTime / 3f);
        }

        void OnTriggerEnter(Collider other)
        {
            var box = other.GetComponent<HitBoxCollider>();
            if (box == null)
            {
                return;
            }

            OnEnterFieldOfView();
        }

        void OnTriggerExit(Collider other)
        {
            var box = other.GetComponent<HitBoxCollider>();
            if (box == null)
            {
                return;
            }

            OnLeaveFieldOfView();
        }

        void Update()
        {
            mTimer.Tick();
        }

    }
}
