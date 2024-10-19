using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    public float radius = 5.0f; // 원의 반지름
    public float radius2 = 5.0f;
    public int segments = 100; // 원을 구성하는 선분의 수
    public float power = 100f;
    public Transform Target;
    public GameObject mMonster;
    public AnimationCurve Curve;
    public Transform obj;
    [Range(0f, 1f)]
    public float i;
    public float interval;
    public bool start;
    float time;
    public UnityEvent Action;
    bool before;
    public float Angle1;
    float Angle => (Angle1 / 360f) * (2 * Mathf.PI);
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Spawn();
        }


        Vector3 pointA = transform.position + new Vector3(Mathf.Cos(Angle) * radius, 0, Mathf.Sin(Angle) * radius);
        Vector3 pointB = transform.position + Vector3.up * Curve.Evaluate(1f) + new Vector3(Mathf.Cos(Angle) * radius2, 0, Mathf.Sin(Angle) * radius2);

        if (start && before != start)
        {
            Action.Invoke();
        }

        if (!start)
        {
            return;
        }

        time += Time.deltaTime;
        i = Mathf.Min(time / interval, 1f);
        // 커브를 따라 점들을 계산하여 선을 그립니다.

        float y = Curve.Evaluate(i);
        Vector3 point = Vector3.Lerp(pointA, pointB, i);
        point.y = y;

        obj.position = point;
        if (i == 1)
        {
            time = 0;
            start = false;
        }
        before = start;
    }

    void OnDrawGizmos()
    {
        var originColor = Gizmos.color;
        Gizmos.color = Color.red; // 원의 색상 설정
        DrawCircleGizmo(transform.position, radius, segments);
        DrawCircleGizmo(transform.position + Vector3.up * Curve.Evaluate(1f), radius2, segments);
        DrawCurve();
        Gizmos.color = originColor;
    }

    static void DrawCircleGizmo(Vector3 position, float radius, int segments)
    {
        Vector3 previousPoint = position + new Vector3(radius, 0, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * 2 * Mathf.PI / segments; // 각도 계산
            Vector3 nextPoint = position + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            Gizmos.DrawLine(previousPoint, nextPoint); // 선분 그리기
            previousPoint = nextPoint; // 이전 점 업데이트
        }

    }

    void Spawn()
    {
        var cube = Instantiate(mMonster);
        cube.SetActive(true);
        var rigid = cube.GetComponent<Rigidbody>();
        var angle = UnityEngine.Random.Range(0, 359f);
        Vector3 nextPoint = transform.position + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
        Vector3 lookAt = Target.position;
        lookAt.y = nextPoint.y;
        cube.transform.position = nextPoint;
        cube.transform.LookAt(lookAt);
        rigid.AddForce((Target.position - nextPoint) * power);
    }

    void DrawCurve()
    {
        var angle = 0;
        Vector3 pointA = transform.position + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
        Vector3 pointB = transform.position + Vector3.up * Curve.Evaluate(1f) + new Vector3(Mathf.Cos(angle) * radius2, 0, Mathf.Sin(angle) * radius2);


        // 커브를 따라 점들을 계산하여 선을 그립니다.
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
