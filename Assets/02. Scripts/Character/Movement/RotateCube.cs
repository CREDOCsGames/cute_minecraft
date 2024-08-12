using System.Collections;
using UnityEngine;

namespace PlatformGame.Character.Movement
{
    [CreateAssetMenu(menuName = "Custom/MovementAction/Rotate")]
    public class RotateCube : MovementAction
    {
        Vector3 forward;
        Vector3 right;
        Vector3 up;
        Vector3 FixedRightAxis;
        Vector3 FixedUpAxis;
        Transform mTransform;
        MonoBehaviour mCoroutine;
        float mRotationAmount = 90f;
        readonly Vector3[] mDirArr = new Vector3[3];
        [SerializeField] float mRotationSpeed = 1f;
        [SerializeField, Range(-1f, 1f)] int mVertical;
        [SerializeField, Range(-1f, 1f)] int mHorizontal;

        public override void PlayAction(Rigidbody rigid, MonoBehaviour coroutine)
        {
            mTransform = rigid.transform;
            mCoroutine = coroutine;

            if (mHorizontal != 0)
            {
                RotateHorizontal(mHorizontal);
            }

            if (mVertical != 0)
            {
                RotateVertical(mVertical);
            }
        }

        void RotateHorizontal(float dir)
        {
            UpdateFixedAxis();
            mRotationAmount = Mathf.Abs(mRotationAmount) * dir;
            mCoroutine.StartCoroutine(RotateObject(FixedUpAxis));
        }

        void RotateVertical(float dir)
        {
            UpdateFixedAxis();
            mRotationAmount = Mathf.Abs(mRotationAmount) * dir;
            mCoroutine.StartCoroutine(RotateObject(FixedRightAxis));
        }

        void UpdateFixedAxis()
        {
            up = Temping(mTransform.up);
            right = Temping(mTransform.right);
            forward = Temping(mTransform.forward);

            mDirArr[0] = up;
            mDirArr[1] = right;
            mDirArr[2] = forward;

            foreach (var dir in mDirArr)
            {
                if (dir.x != 0)
                {
                    FixedRightAxis = dir;
                    break;
                }

                if (dir.y != 0)
                {
                    FixedUpAxis = dir;
                }
            }
        }

        static Vector3 Temping(Vector3 vector)
        {
            var max = Mathf.Max(vector.x * vector.x, vector.y * vector.y, vector.z * vector.z);

            if (max.Equals(vector.x * vector.x))
            {
                vector = Vector3.right * (vector.x > 0 ? 1 : -1);
            }
            else if (max.Equals(vector.y * vector.y))
            {
                vector = Vector3.up * (vector.y > 0 ? 1 : -1);
            }
            else
            {
                vector = Vector3.forward * (vector.z > 0 ? 1 : -1);
                ;
            }

            return vector;
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