using PlatformGame.Character.Movement;
using UnityEngine;
using static PlatformGame.Character.Movement.MovementHelper;

namespace PlatformGame.Character
{
    public class Role : MonoBehaviour
    {
        public Transform Transform { get; protected set; }
        [SerializeField] ChatBalloon mChat;
        public ChatBalloon Chat => mChat;
        [SerializeField] ID mID;
        public ID ID => mID;
        bool mbStop = true;
        [SerializeField] protected Formation mFormation;
        [SerializeField] TransformBaseMovement mTrace;

        public void SetTransform(Transform transform)
        {
            mFormation.Transform = transform;
            Transform = transform;
        }

        void TraceFormation()
        {
            StopAllCoroutines();
            StartCoroutine(mTrace.Move(transform, mFormation.Transform));
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
            if (Transform == null)
            {
                SetTransform(transform);
            }
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