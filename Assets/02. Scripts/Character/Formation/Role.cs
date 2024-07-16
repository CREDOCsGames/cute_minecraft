using PlatformGame.Character.Movement;
using UnityEngine;
using static PlatformGame.Character.Movement.MovementHelper;

namespace PlatformGame.Character
{
    public class Role : MonoBehaviour
    {
        protected Transform mSwapTransform;
        protected Transform mOriginTransform;
        bool mbStop = true;
        [SerializeField] protected Formation mFormation;
        [SerializeField] TransformBaseMovement mTrace;
        [SerializeField] TransformBaseMovement mSwapTrace;

        public virtual void DoAction()
        {
            Debug.LogWarning("Calling deprecated methods.");
        }

        protected void SwapTransform()
        {
            (mFormation.Transform, mSwapTransform) = (mSwapTransform, mFormation.Transform);
            (mTrace, mSwapTrace) = (mSwapTrace, mTrace);
        }

        public void SetTransform(Transform transform)
        {
            mFormation.Transform = transform;
            mOriginTransform = transform;
        }

        void TraceFormation()
        {
            StopAllCoroutines();
            StartCoroutine(mTrace.Move(transform, mFormation.Transform, true));
        }

        void StopTrace()
        {
            mbStop = true;
        }

        void StartTrace()
        {
            mbStop = false;
        }

        protected virtual void Awake()
        {
            SetTransform(transform);
            mFormation.Trace = TraceFormation;
            mFormation.IsStoped = () => mbStop;
            mFormation.IsReached = () => IsNearByDistance(transform, mFormation.Transform);
            mFormation.OnStopMoveEvent.AddListener(StopTrace);
            mFormation.OnStartMoveEvent.AddListener(StartTrace);
        }

        protected virtual void Update()
        {
            mFormation.UpdateBehaviour();
        }

    }
}