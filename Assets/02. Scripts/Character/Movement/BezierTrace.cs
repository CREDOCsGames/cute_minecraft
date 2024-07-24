using System.Collections;
using UnityEngine;

namespace PlatformGame.Character.Movement
{
    [CreateAssetMenu(menuName = "Custom/MovementAction/BazierTrace")]
    class BezierTrace : TransformBaseMovement
    {
        [SerializeField] AnimationCurve curve;
        [SerializeField] float duration;
        public override IEnumerator Move(Transform start, Transform end, bool repeat = false)
        {
            do
            {
                float time = 0;
                Vector3 controlPoint;
                var startPos = start.position;
                var endPos = end.position;
                controlPoint.x = Random.Range(startPos.x, endPos.x);
                controlPoint.y = Random.Range(startPos.y, endPos.y);
                controlPoint.z = Random.Range(startPos.z, endPos.z);

                while (time < duration)
                {
                    time += Time.fixedDeltaTime;
                    var bezierPosition = CalculateBezierPoint(startPos, controlPoint, end.position, (time * curve.Evaluate(time / duration)) / duration);
                    start.position = bezierPosition;
                    yield return new WaitForFixedUpdate();
                }
                start.position = end.position;
            }
            while (repeat);
        }
        public static Vector3 CalculateBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            var u = 1 - t;
            var tt = t * t;
            var uu = u * u;

            var point = uu * p0;
            point += 2 * u * t * p1;
            point += tt * p2;

            return point;
        }
    }
}
