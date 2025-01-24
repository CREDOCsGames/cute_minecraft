using System;
using System.Collections;
using UnityEngine;
using Util;

namespace Battle
{
    [Serializable]
    public class JumpController
    {
        public Vector3 StartPoint;
        public Vector3 EndPoint;
        public AnimationCurve JumpCurve;
        public event Action<Transform> OnEndEvent;
        public event Action<Transform> OnFailEvent;
        public event Action<Transform> OnStartEvent;
        public event Action<Transform> OnReachedEvent;
        private bool _bInterrupt;

        public void Stop()
        {
            _bInterrupt = true;
        }
        public IEnumerator Move(Transform Target)
        {
            if (Target == null)
            {
                yield break;
            }
            var target = Target;
            var startPoint = StartPoint;
            var endPoint = EndPoint;
            var lookDir = endPoint;
            var time = 0f;
            var progress = 0f;
            var moveTime = JumpCurve.keys[^1].time - JumpCurve.keys[0].time;
            _bInterrupt = false;

            target.position = startPoint;
            lookDir.y = startPoint.y;
            Target.LookAt(lookDir);

            OnStartEvent?.Invoke(target);
            while (progress < 1f)
            {
                if (_bInterrupt)
                {
                    break;
                }
                time += Time.deltaTime;
                progress = Mathf.Clamp01(time / moveTime);
                target.position = DrawGizmos.Lerp(startPoint, endPoint, progress, JumpCurve);
                yield return null;
            }

            if (!_bInterrupt)
            {
                target.position = endPoint;
                OnReachedEvent?.Invoke(target);
            }
            else
            {
                OnFailEvent?.Invoke(target);
            }
            OnEndEvent?.Invoke(target);
        }

    }
}