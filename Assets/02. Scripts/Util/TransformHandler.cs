using UnityEngine;

namespace Util
{
    public class TransformHandler : MonoBehaviour
    {
        Vector3 mFirstLocalPos;
        Quaternion mFirstLocalRot;
        Vector3 mFirstPos;
        Quaternion mFirstRot;

        public void SetLocalPositionX(float x)
        {
            var vector = transform.localPosition;
            vector.x = x;
            transform.localPosition = vector;
        }

        public void SetLocalPositionY(float y)
        {
            var vector = transform.localPosition;
            vector.y = y;
            transform.localPosition = vector;
        }

        public void SetLocalPositionZ(float z)
        {
            var vector = transform.localPosition;
            vector.z = z;
            transform.localPosition = vector;
        }

        public void SetLocalRotationX(float x)
        {
            var vector = transform.eulerAngles;
            vector.x = x;
            transform.localEulerAngles = vector;
        }

        public void SetLocalRotationY(float y)
        {
            var vector = transform.eulerAngles;
            vector.y = y;
            transform.localEulerAngles = vector;
        }

        public void SetLocalRotationZ(float z)
        {
            var vector = transform.eulerAngles;
            vector.z = z;
            transform.localEulerAngles = vector;
        }

        public void DefaultPosition()
        {
            transform.localPosition = Vector3.zero;
        }

        public void DefaultRotation()
        {
            transform.localRotation = Quaternion.identity;
        }

        public void SetLocalRotation(Vector3 rot)
        {
            transform.eulerAngles = rot;
        }

        public void SetFirstLocalPosition()
        {
            transform.localPosition = mFirstLocalPos;
        }

        public void SetFirstPosition()
        {
            transform.position = mFirstLocalPos;
        }

        public void SetFirstPositionAtParent()
        {
            var parent = transform;
            while (true)
            {
                parent = parent.parent;
                if (parent.parent == null)
                {
                    break;
                }
            }

            parent.position = mFirstPos;
        }

        public void SetFirstRotation()
        {
            transform.rotation = mFirstRot;
        }

        public void SetFirstLoaclRotation()
        {
            transform.localRotation = mFirstLocalRot;
        }

        public void SetPosition(Transform transform)
        {
            this.transform.position = transform.position;
        }

        public void SetRotation(Transform transform)
        {
            this.transform.rotation = transform.rotation;
        }

        public void SetPrarent(Transform transform)
        {
            transform.SetParent(transform);
        }

        public void SetParentNull()
        {
            transform.parent = null;
        }

        void Awake()
        {
            mFirstLocalPos = transform.localPosition;
            mFirstLocalRot = transform.localRotation;
            mFirstPos = transform.position;
            mFirstRot = transform.rotation;
        }
    }
}