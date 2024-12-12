using UnityEngine;
using Util;
using System;
using System.Collections;

namespace Battle
{
    [Serializable]
    public class JumpController
    {
        public Vector3 StartPoint;
        public Vector3 EndPoint;
        public AnimationCurve JumpCurve;
        public event Action<Transform> OnEndEvent;
        public event Action<Transform> OnStartEvent;
        private Transform _target;
        public bool IsActing { get; private set; }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public IEnumerator Move()
        {
            if (_target == null)
            {
                yield break;
            }
            IsActing = true;
            var time = 0f;
            var progress = 0f;
            var moveTime = JumpCurve.keys[^1].time - JumpCurve.keys[0].time;

            OnStartEvent?.Invoke(_target);
            while (progress < 1f)
            {
                time += Time.deltaTime;
                progress = Mathf.Clamp01(time / moveTime);
                _target.position = DrawGizmos.Lerp(StartPoint, EndPoint, progress, JumpCurve);
                yield return null;
            }

            _target.position = EndPoint;
            IsActing = false;
            OnEndEvent?.Invoke(_target);
        }

    }
}