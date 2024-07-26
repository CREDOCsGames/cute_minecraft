using System;
using UnityEngine;
using UnityEngine.Events;

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
                OnChangeFormation(mTransform);
            }
        }
        public UnityAction Trace;
        public Func<bool> IsStoped = () => true;
        public Func<bool> IsReached = () => false;
        public Func<bool> BehaviourConditoin = () => false;
        public UnityAction Behaviour;
        public UnityEvent OnStopMoveEvent;
        public UnityEvent OnStartMoveEvent;
        public UnityEvent OnReachFormationEvent;
        public UnityEvent<Transform> OnChangeFormationEvent;
        public UnityEvent OnBehaviourEvent;

        public void UpdateBehaviour()
        {
            Debug.Assert(BehaviourConditoin != null);
            if (BehaviourConditoin())
            {
                OnBehaviour();
                return;
            }

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

        void OnChangeFormation(Transform transform)
        {
            OnStopMove();
            OnChangeFormationEvent.Invoke(transform);
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

        void OnBehaviour()
        {
            Behaviour.Invoke();
            OnBehaviourEvent.Invoke();
        }

    }
}