using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    public float Radius = 5.0f;
    public float Radius2 = 5.0f;
    public int Segments = 100;
    public AnimationCurve Curve;
    public Transform Monster;
    public float interval;
    public float Angle;
    public bool Start;
    public UnityEvent SpawnAction;
    public UnityEvent ReachActoin;
    float time;
    bool before;
    float Radian => (Angle / 360f) * (2 * Mathf.PI);

    public void SetStart()
    {
        Start = true;
    }

    void Update()
    {
        Vector3 pointA = transform.position + new Vector3(Mathf.Cos(Radian) * Radius, 0, Mathf.Sin(Radian) * Radius);
        Vector3 pointB = transform.position + Vector3.up * Curve.Evaluate(1f) + new Vector3(Mathf.Cos(Radian) * Radius2, 0, Mathf.Sin(Radian) * Radius2);

        if (Start && before != Start)
        {
            SpawnAction.Invoke();
            Monster.transform.LookAt(pointB - pointA);
        }

        if (!Start)
        {
            return;
        }

        time += Time.deltaTime;
        var i = Mathf.Min(time / interval, 1f);
        float y = Curve.Evaluate(i);
        Vector3 point = Vector3.Lerp(pointA, pointB, i);
        point.y = y;

        Monster.position = point;
        if (i == 1)
        {
            time = 0;
            Start = false;
            ReachActoin.Invoke();
        }
        before = Start;
    }

    void OnDrawGizmos()
    {
        var originColor = Gizmos.color;
        Gizmos.color = Color.red;
        DrawCircleGizmo(transform.position, Radius, Segments);
        DrawCircleGizmo(transform.position + Vector3.up * Curve.Evaluate(1f), Radius2, Segments);
        DrawCurve();
        Gizmos.color = originColor;
    }

    static void DrawCircleGizmo(Vector3 position, float radius, int segments)
    {
        Vector3 previousPoint = position + new Vector3(radius, 0, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * 2 * Mathf.PI / segments;
            Vector3 nextPoint = position + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            Gizmos.DrawLine(previousPoint, nextPoint);
            previousPoint = nextPoint;
        }

    }

    void DrawCurve()
    {
        var angle = 0;
        Vector3 pointA = transform.position + new Vector3(Mathf.Cos(angle) * Radius, 0, Mathf.Sin(angle) * Radius);
        Vector3 pointB = transform.position + Vector3.up * Curve.Evaluate(1f) + new Vector3(Mathf.Cos(angle) * Radius2, 0, Mathf.Sin(angle) * Radius2);


        for (float t = 0; t <= 1; t += 0.1f)
        {
            float y = Curve.Evaluate(t);
            Vector3 point = Vector3.Lerp(pointA, pointB, t);
            point.y = y;

            if (t > 0)
            {
                Vector3 previousPoint = Vector3.Lerp(pointA, pointB, t - 0.1f);
                previousPoint.y = Curve.Evaluate(t - 0.1f);
                Gizmos.DrawLine(previousPoint, point);
            }
        }
    }

}
