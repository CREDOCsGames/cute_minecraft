using System.Collections;
using UnityEngine;

namespace Movement
{
    [CreateAssetMenu(menuName = "Custom/MovementAction/Rotate")]
    public class Rotate : MovementAction
    {
        private static readonly Vector3 FIXED_RIGHT_AXIS = Vector3.right;
        private static readonly Vector3 FIXED_UP_AXIS = Vector3.up;
        private static readonly Vector3 FIXED_FORWARD_AXIS = Vector3.forward;
        private Transform _transform;
        private MonoBehaviour _coroutine;
        private float _rotationAmount = 90f;
        [SerializeField] private float _rotationSpeed = 1f;
        [SerializeField, Range(-1f, 1f)] private int _axisX;
        [SerializeField, Range(-1f, 1f)] private int _axisY;
        [SerializeField, Range(-1f, 1f)] private int _axisZ;

        public override void PlayAction(Rigidbody rigid, MonoBehaviour coroutine)
        {
            _transform = rigid.transform;
            _coroutine = coroutine;

            if (_axisY != 0)
            {
                RotateHorizontal(_axisY);
            }

            if (_axisX != 0)
            {
                RotateVertical(_axisX);
            }

            if (_axisZ != 0)
            {
                RotateForward(_axisZ);
            }
        }

        private void RotateHorizontal(float dir)
        {
            _rotationAmount = Mathf.Abs(_rotationAmount) * dir;
            _coroutine.StartCoroutine(RotateObject(FIXED_UP_AXIS));
        }

        private void RotateVertical(float dir)
        {
            _rotationAmount = Mathf.Abs(_rotationAmount) * dir;
            _coroutine.StartCoroutine(RotateObject(FIXED_RIGHT_AXIS));
        }

        private void RotateForward(float dir)
        {
            _rotationAmount = Mathf.Abs(_rotationAmount) * dir;
            _coroutine.StartCoroutine(RotateObject(FIXED_FORWARD_AXIS));
        }

        private IEnumerator RotateObject(Vector3 axis)
        {
            var time = 0f;
            var transform = _transform;
            var startRotation = transform.rotation;
            var endRotation = Quaternion.AngleAxis(_rotationAmount, axis) * startRotation;

            while (time < 1f)
            {
                transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
                time += Time.deltaTime * _rotationSpeed;
                yield return null;
            }

            transform.rotation = endRotation;
        }
    }
}