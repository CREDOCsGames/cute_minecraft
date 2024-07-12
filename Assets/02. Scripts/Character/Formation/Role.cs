using PlatformGame.Character.Movement;
using UnityEngine;
using static PlatformGame.Character.Movement.MovementHelper;

namespace PlatformGame.Character
{
    public class Role : MonoBehaviour
    {
        bool mbStop = true;
        [SerializeField] Formation mFormation;
        [SerializeField] TransformBaseMovement mTrace;

        public void SetTransform(Transform transform)
        {
            mFormation.Transform = transform;
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

        void Awake()
        {
            mFormation.Transform = transform;
            mFormation.Trace = TraceFormation;
            mFormation.IsStoped = () => mbStop;
            mFormation.IsReached = () => IsNearByDistance(transform, mFormation.Transform);
            mFormation.OnStopMoveEvent.AddListener(StopTrace);
            mFormation.OnStartMoveEvent.AddListener(StartTrace);
        }

        void Update()
        {
            mFormation.UpdateBehaviour();
        }

    }
}
