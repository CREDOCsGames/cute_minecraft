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
        public event Action<Transform> OnStartEvent;

        public IEnumerator Move(Transform Target)
        {
            Transform target = Target;
            if (target == null)
            {
                yield break;
            }

            var time = 0f;
            var progress = 0f;
            var moveTime = JumpCurve.keys[^1].time - JumpCurve.keys[0].time;
            var startPoint = StartPoint;
            var endPoint = EndPoint;

            target.position = startPoint;
            var lookDir = endPoint;
            lookDir.y = startPoint.y;
            Target.LookAt(lookDir);
            OnStartEvent?.Invoke(target);
            while (progress < 1f)
            {
                time += Time.deltaTime;
                progress = Mathf.Clamp01(time / moveTime);
                target.position = DrawGizmos.Lerp(startPoint, endPoint, progress, JumpCurve);
                yield return null;
            }
            target.position = endPoint;
            OnEndEvent?.Invoke(target);
        }

    }
}