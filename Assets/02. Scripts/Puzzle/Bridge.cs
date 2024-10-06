using UnityEngine;

namespace Puzzle
{
    public class Bridge
    {
        bool IsConnected => mInstance;
        bool IsConnectable => Vector3.Distance(PointA, PointB) <= mLimit && Vector3.Distance(PointA, PointB) > 5;
        Vector3 PointA { get; set; }
        Vector3 PointB { get; set; }
        GameObject mInstance;
        readonly float mLimit;
        static readonly Material M_LED = Resources.Load<Material>("Materials/M_LED_Bridge");

        static readonly Vector3[] mDirections =
        {
            new Vector3(1, 1, 0),
            new Vector3(0, 1, -1),
            new Vector3(-1, 1, 0),
            new Vector3(0, 1, 1)
        };

        public Bridge(float limit)
        {
            mLimit = limit;
        }

        public void SetConnectionPoints(Bounds basis, Bounds area)
        {
            basis.extents /= 2;
            area.extents /= 2;

            var line = new Vector2(area.center.x, area.center.z) - new Vector2(basis.center.x, basis.center.z);
            var angle = Mathf.Atan2(line.x, line.y) * Mathf.Rad2Deg - 45;
            var indexBasis = (int)Mathf.Repeat(angle, 360f) / 90;
            var indexArea = (int)Mathf.Repeat(indexBasis + 2, 4);

            PointA = basis.center + Vector3.Scale(mDirections[indexBasis], basis.extents);
            PointB = area.center + Vector3.Scale(mDirections[indexArea], area.extents);
        }

        public void ConnectAtoB()
        {
            if (!IsConnectable)
            {
                return;
            }

            mInstance = GameObject.CreatePrimitive(PrimitiveType.Cube);
            mInstance.GetComponent<MeshRenderer>().material = M_LED;
            mInstance.layer = LayerMask.NameToLayer("Bridge");

            var size = new Vector3(1.5f, 0.5f, Vector3.Distance(PointA, PointB) + 1);
            mInstance.transform.localScale = size;

            var center = (PointA + PointB) / 2 - Vector3.up * size.y / 2;
            mInstance.transform.position = center;

            mInstance.transform.LookAt(PointB);

            CerateColliderWall();
        }

        void CerateColliderWall()
        {
            var obj = new GameObject();
            obj.transform.SetParent(mInstance.transform);

            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.up * Mathf.Abs(PointA.y - PointB.y) + Vector3.right + Vector3.forward;
            obj.transform.localEulerAngles = Vector3.left * Mathf.Abs(obj.transform.parent.eulerAngles.x);

            var scale = new Vector3(1 / 1.5f * 0.1f,
                (2 / mInstance.transform.lossyScale.y) * Mathf.Abs(PointA.y - PointB.y), 1);

            var left = obj.AddComponent<BoxCollider>();
            left.size = scale;
            left.center = Vector3.left * 0.5f;
            var right = obj.AddComponent<BoxCollider>();
            right.center = Vector3.right * 0.5f;
            right.size = scale;
        }

        public void Disconnect()
        {
            if (!IsConnected)
            {
                return;
            }

            Object.Destroy(mInstance);
            mInstance = null;
        }
    }
}