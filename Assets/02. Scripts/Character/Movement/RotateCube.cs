using System.Collections;
using UnityEngine;

namespace PlatformGame.Character.Movement
{
    [CreateAssetMenu(menuName = "Custom/MovementAction/Rotate")]
    public class RotateCube : MovementAction
    {
        Vector3 FixedRightAxis = Vector3.right;
        Vector3 FixedUpAxis = Vector3.up;
        Vector3 FixedForwardAxis = Vector3.forward;
        Transform mTransform;
        MonoBehaviour mCoroutine;
        float mRotationAmount = 90f;
        [SerializeField] float mRotationSpeed = 1f;
        [SerializeField, Range(-1f, 1f)] int AxisX;
        [SerializeField, Range(-1f, 1f)] int AxisY;
        [SerializeField, Range(-1f, 1f)] int AxisZ;

        public override void PlayAction(Rigidbody rigid, MonoBehaviour coroutine)
        {
            mTransform = rigid.transform;
            mCoroutine = coroutine;

            if (AxisY != 0)
            {
                RotateHorizontal(AxisY);
            }

            if (AxisX != 0)
            {
                RotateVertical(AxisX);
            }

            if(AxisZ != 0)
            {
                RotateForward(AxisZ);
            }
        }

        void RotateHorizontal(float dir)
        {
            mRotationAmount = Mathf.Abs(mRotationAmount) * dir;
            mCoroutine.StartCoroutine(RotateObject(FixedUpAxis));
        }

        void RotateVertical(float dir)
        {
            mRotationAmount = Mathf.Abs(mRotationAmount) * dir;
            mCoroutine.StartCoroutine(RotateObject(FixedRightAxis));
        }

        void RotateForward(float dir)
        {
            mRotationAmount = Mathf.Abs(mRotationAmount) * dir;
            mCoroutine.StartCoroutine(RotateObject(FixedForwardAxis));
        }

        IEnumerator RotateObject(Vector3 axis)
        {
            var time = 0f;
            var transform = mTransform;
            var startRotation = transform.rotation;
            var endRotation = Quaternion.AngleAxis(mRotationAmount, axis) * startRotation;

            while (time < 1f)
            {
                transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
                time += Time.deltaTime * mRotationSpeed;
                yield return null;
            }

            transform.rotation = endRotation;
        }
    }
}