using UnityEngine;

namespace Util
{
    public static class DrawGizmos
    {
        public static void DrawCircle(Vector3 position, float radius, byte segmentsCount = 100)
        {
            float angle;
            Vector3 nextPoint;
            Vector3 prevPoint = position + Vector3.right * radius;

            for (int i = 1; i <= segmentsCount; i++)
            {
                angle = i * 2 * Mathf.PI / segmentsCount;
                nextPoint = position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
                Gizmos.DrawLine(prevPoint, nextPoint);
                prevPoint = nextPoint;
            }
        }

        public static void DrawLines(Vector3[] positions)
        {
            for (int i = 0; i < positions.Length - 1; i++)
            {
                Gizmos.DrawLine(positions[i], positions[i + 1]);
            }
        }

        public static void DrawCurve(Vector3 startPosition, Vector3 endPosition, AnimationCurve curve, byte segmentsCount = 10)
        {
            if (curve.length < 2)
            {
                return;
            }

            Vector3 currentPoint;
            Vector3 prevPoint = startPosition;
            for (int i = 1; i <= segmentsCount; i++)
            {
                float t = (float)i / (float)segmentsCount;
                currentPoint = Lerp(startPosition, endPosition, t, curve);
                Gizmos.DrawLine(prevPoint, currentPoint);
                prevPoint = currentPoint;
            }
        }

        public static Vector3 Lerp(Vector3 a, Vector3 b, float t, AnimationCurve curve)
        {
            if (curve.length < 2)
            {
                return a;
            }
            t = Mathf.Clamp01(t);

            var intervalTime = curve.keys[^1].time - curve.keys[0].time;
            var intervalValue = curve.keys[^1].value - curve.keys[0].value;
            var intervalHeight = b.y - a.y;

            var point = Vector3.Lerp(a, b, t);
            point.y = a.y + (curve.Evaluate(curve.keys[0].time + t * intervalTime) - curve.keys[0].value) * (intervalHeight / intervalValue);
            return point;
        }

    }
}
