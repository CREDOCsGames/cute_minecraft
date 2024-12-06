using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Util;

public class CurveMoverComponent : MonoBehaviour
{
    public float StartRadius = 5.0f;
    public float EndRadius = 3.0f;
    [Range(0, 359)][SerializeField] private float _positionAngle = 0f;
    [SerializeField] private AnimationCurve _jumpCurve;
    [SerializeField] private UnityEvent _onStartEvent;
    [SerializeField] private UnityEvent _onEndEvent;
    [SerializeField] private Transform _target;
    private float _positionAngleInRadians => _positionAngle * Mathf.Deg2Rad;

    public void StartMove(Transform transform)
    {
        StartCoroutine(Move(transform));
    }

    private Vector3 CalculatePointOnCircle(float radius, float heightOffset = 0f)
    {
        return transform.position
            + new Vector3(Mathf.Cos(_positionAngleInRadians) * radius,
                          heightOffset,
                          Mathf.Sin(_positionAngleInRadians) * radius);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartMove(_target);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_jumpCurve.keys.Length < 2)
        {
            return;
        }

        Color originColor = Gizmos.color;

        Gizmos.color = Color.yellow;
        DrawGizmos.DrawCircle(transform.position + Vector3.up * _jumpCurve.keys[0].value, StartRadius);
        DrawGizmos.DrawCircle(transform.position + Vector3.up * _jumpCurve.keys[^1].value, EndRadius);

        Gizmos.color = Color.red;
        var startPoint = CalculatePointOnCircle(StartRadius, _jumpCurve.keys[0].value);
        var endPoint = CalculatePointOnCircle(EndRadius, _jumpCurve.keys[^1].value);
        DrawGizmos.DrawCurve(startPoint, endPoint, _jumpCurve);

        Gizmos.color = originColor;
    }

    private IEnumerator Move(Transform moverTarget)
    {
        var time = 0f;
        var progress = 0f;
        var moveTime = _jumpCurve.keys[^1].time - _jumpCurve.keys[0].time;
        var startPosition = CalculatePointOnCircle(StartRadius, _jumpCurve.keys[0].value);
        var endPosition = CalculatePointOnCircle(EndRadius, _jumpCurve.keys[^1].value);

        _onStartEvent.Invoke();
        while (progress < 1f)
        {
            time += Time.deltaTime;
            progress = Mathf.Clamp01(time / moveTime);
            moverTarget.position = DrawGizmos.Lerp(startPosition, endPosition, progress, _jumpCurve);
            yield return null;
        }

        moverTarget.position = endPosition;
        _onEndEvent.Invoke();
    }

}
