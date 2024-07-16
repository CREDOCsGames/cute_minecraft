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
            float time;
            Vector3 controlPoint;
            Vector3 startPos;
            do
            {
                time = 0;
                controlPoint.x = Random.Range(start.position.x, end.position.x);
                controlPoint.y = Random.Range(start.position.y, end.position.y);
                controlPoint.z = Random.Range(start.position.z, end.position.z);
                startPos = start.position;

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
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            Vector3 point = uu * p0;
            point += 2 * u * t * p1;
            point += tt * p2;

            return point;
        }
    }
}
