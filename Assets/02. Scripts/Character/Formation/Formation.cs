using UnityEngine.Events;
using UnityEngine;
using System;

namespace PlatformGame.Character
{
    [Serializable]
    public class Formation
    {
        Transform mTransform;
        public Transform Transform
        {
            get => mTransform;
            set
            {
                mTransform = value;
                OnChangeFormation();
            }
        }
        public UnityAction Trace;
        public Func<bool> IsStoped;
        public Func<bool> IsReached;
        public UnityEvent OnReachFormationEvent;
        public UnityEvent OnChangeFormationEvent;
        public UnityEvent OnStopMoveEvent;
        public UnityEvent OnStartMoveEvent;

        public void UpdateBehaviour()
        {
#if DEVELOPMENT
            var isReached = IsReached();
            var isStoped = IsStoped();
#endif
            if (IsReached() && !IsStoped())
            {
                OnReachFormation();
                return;
            }

            if (IsStoped() && !IsReached())
            {
                OnMoveToFormation();
            }
        }

        void OnReachFormation()
        {
            OnStopMove();
            OnReachFormationEvent.Invoke();
        }

        void OnChangeFormation()
        {
            OnStopMove();
            OnChangeFormationEvent.Invoke();
        }

        void OnStopMove()
        {
            OnStopMoveEvent.Invoke();
        }

        void OnStartMove()
        {
            OnStartMoveEvent.Invoke();
        }

        void OnMoveToFormation()
        {
            Trace.Invoke();
            OnStartMove();
        }

    }
}