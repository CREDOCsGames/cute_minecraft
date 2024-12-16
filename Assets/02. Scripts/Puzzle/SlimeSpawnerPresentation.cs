using Battle;
using Controller;
using Puzzle;
using System.Linq;
using UnityEngine;
using Util;
namespace NW
{
    public class SlimeSpawnerPresentation : IPresentation
    {
        public float StartRadius = 5.0f;
        public float EndRadius = 3.0f;
        private float _positionAngle = 0f;
        private readonly Transform _baseTransform;
        private readonly AnimationCurve _jumpCurve;
        private readonly Transform _slime;
        private readonly JumpController _jumpController = new();
        private float _positionAngleInRadians => _positionAngle * Mathf.Deg2Rad;

        public SlimeSpawnerPresentation(Transform baseTransform, Transform slime, AnimationCurve jumpCurve)
        {
            _jumpController.JumpCurve = jumpCurve;
            _jumpCurve = jumpCurve;
            _baseTransform = baseTransform;
            _slime = slime;
            _jumpController.OnStartEvent += PlaySpawnMotion;
            _jumpController.OnEndEvent += StartBehavior;
        }
        public void InstreamData(byte[] data)
        {
            if (data == MonsterReader.SLIME_SPAWN)
            {
                SpawnSlime();
            }
        }
        private void SpawnSlime()
        {
            var go = GameObject.Instantiate(_slime);
            _positionAngle = Random.Range(0, 360);
            _jumpController.StartPoint = CalculatePointOnCircle(StartRadius, _jumpCurve.keys[0].value);
            _jumpController.EndPoint = CalculatePointOnCircle(EndRadius, _jumpCurve.keys[^1].value);
            go.transform.position = _jumpController.StartPoint;
            go.transform.LookAt(_jumpController.EndPoint);
            CoroutineRunner.InvokeDelayAction(() => CoroutineRunner.Instance.StartCoroutine(_jumpController.Move(go.transform)), 4f);
        }
        private Vector3 CalculatePointOnCircle(float radius, float heightOffset = 0f)
        {
            return _baseTransform.position
                + new Vector3(Mathf.Cos(_positionAngleInRadians) * radius,
                              heightOffset,
                              Mathf.Sin(_positionAngleInRadians) * radius);
        }
        private void PlaySpawnMotion(Transform transform)
        {
            if (transform.TryGetComponent<MonsterComponent>(out var slime))
            {
                slime.Hit();
            }
        }
        private void StartBehavior(Transform transform)
        {
            if (transform.TryGetComponent<MonsterComponent>(out var slime))
            {
                var controller = new Controller.MonsterState();
                var list = GameObject.FindObjectsOfType<Flower>().ToList();
                var target = list[Random.Range(0, list.Count)].transform;
                controller.StartTrace(target);
                slime._character.ChangeController(controller);
            }
        }
    }

}
