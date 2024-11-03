using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Util;

public class CurveMoverComponent : MonoBehaviour
{
    public float StartRadius = 5.0f;
    public float EndRadius = 3.0f;
    [Range(0, 359)] public float PositionAngle = 0f;
    public AnimationCurve JumpCurve;
    public UnityEvent OnStartEvent;
    public UnityEvent OnEndEvent;
    public Transform t;
    float mPositionAngleInRadians => PositionAngle * Mathf.Deg2Rad;

    public void StartMove(Transform transform)
    {
        StartCoroutine(Move(transform));
    }

    Vector3 CalculatePointOnCircle(float radius, float heightOffset = 0f)
    {
        return transform.position
            + new Vector3(Mathf.Cos(mPositionAngleInRadians) * radius,
                          heightOffset,
                          Mathf.Sin(mPositionAngleInRadians) * radius);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartMove(t);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (JumpCurve.keys.Length < 2)
        {
            return;
        }

        Color originColor = Gizmos.color;

        Gizmos.color = Color.yellow;
        DrawGizmos.DrawCircle(transform.position + Vector3.up * JumpCurve.keys[0].value, StartRadius);
        DrawGizmos.DrawCircle(transform.position + Vector3.up * JumpCurve.keys[^1].value, EndRadius);

        Gizmos.color = Color.red;
        var startPoint = CalculatePointOnCircle(StartRadius, JumpCurve.keys[0].value);
        var endPoint = CalculatePointOnCircle(EndRadius, JumpCurve.keys[^1].value);
        DrawGizmos.DrawCurve(startPoint, endPoint, JumpCurve);

        Gizmos.color = originColor;
    }

    IEnumerator Move(Transform moverTarget)
    {
        var time = 0f;
        var progress = 0f;
        var moveTime = JumpCurve.keys[^1].time - JumpCurve.keys[0].time;
        var startPosition = CalculatePointOnCircle(StartRadius, JumpCurve.keys[0].value);
        var endPosition = CalculatePointOnCircle(EndRadius, JumpCurve.keys[^1].value);

        OnStartEvent.Invoke();
        while (progress < 1f)
        {
            time += Time.deltaTime;
            progress = Mathf.Clamp01(time / moveTime);
            moverTarget.position = DrawGizmos.Lerp(startPosition, endPosition, progress, JumpCurve);
            yield return null;
        }

        moverTarget.position = endPosition;
        OnEndEvent.Invoke();
    }

}
