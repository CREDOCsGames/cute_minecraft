using Battle;
using Puzzle;
using UnityEngine;
using Util;

public class SlimeSpawnerPresentation : IPresentation
{
    public float StartRadius = 5.0f;
    public float EndRadius = 3.0f;
    private float _positionAngle = 0f;
    private Transform _baseTransform;
    private readonly JumpController _jumpController = new();
    private float _positionAngleInRadians => _positionAngle * Mathf.Deg2Rad;
    private AnimationCurve _jumpCurve;

    private void SpawnSlime()
    {
        _jumpController.StartPoint = CalculatePointOnCircle(StartRadius, _jumpCurve.keys[0].value);
        _jumpController.EndPoint = CalculatePointOnCircle(EndRadius, _jumpCurve.keys[^1].value);
        CoroutineRunner.Instance.StartCoroutine(_jumpController.Move());
    }
    private Vector3 CalculatePointOnCircle(float radius, float heightOffset = 0f)
    {
        return _baseTransform.position
            + new Vector3(Mathf.Cos(_positionAngleInRadians) * radius,
                          heightOffset,
                          Mathf.Sin(_positionAngleInRadians) * radius);
    }
    public SlimeSpawnerPresentation(Transform baseTransform, Transform slime, AnimationCurve jumpCurve)
    {
        _jumpController.JumpCurve = jumpCurve;
        _jumpController.SetTarget(slime);
        _jumpCurve = jumpCurve;
        _baseTransform = baseTransform;
    }

    private static readonly byte[] SLIME_SPAWN = { (byte)10 };
    public void InstreamData(byte[] data)
    {
        if (data == SLIME_SPAWN)
        {
            SpawnSlime();
        }
    }
}
