using UnityEngine;

namespace PlatformGame.Contents.Puzzle
{
    public class Bridge
    {
        public bool IsConnected => mInstance;
        Vector3 mPointA;
        Vector3 mPointB;
        GameObject mInstance;
        float mLimit;
        static readonly Material M_LED = Resources.Load<Material>("Materials/M_LED_Bridge");

        static readonly Vector3[] mDirections =
        {
            new Vector3(1,1,0),
            new Vector3(0,1,-1),
            new Vector3(-1,1,0),
            new Vector3(0,1,1)
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

            mPointA = basis.center + Vector3.Scale(mDirections[indexBasis], basis.extents);
            mPointB = area.center + Vector3.Scale(mDirections[indexArea], area.extents);
        }

        public void ConnectAtoB()
        {
            if (Vector3.Distance(mPointA, mPointB) > mLimit)
            {
                return;
            }

            mInstance = GameObject.CreatePrimitive(PrimitiveType.Cube);
            mInstance.GetComponent<MeshRenderer>().material = M_LED;

            Vector3 size = new Vector3(1, 1, Vector3.Distance(mPointA, mPointB) + 2);
            mInstance.transform.localScale = size;

            Vector3 center = (mPointA + mPointB) / 2 - Vector3.up * size.y / 2;
            mInstance.transform.position = center;

            mInstance.transform.LookAt(mPointB);

            CerateWall();
        }

        void CerateWall()
        {
            var obj = new GameObject();
            obj.transform.SetParent(mInstance.transform);

            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.up * Mathf.Abs(mPointA.y - mPointB.y) + Vector3.right + Vector3.forward;
            obj.transform.localEulerAngles = Vector3.left * Mathf.Abs(obj.transform.parent.eulerAngles.x);

            var left = obj.AddComponent<BoxCollider>();
            left.center = Vector3.right;
            var right = obj.AddComponent<BoxCollider>();
            right.center = Vector3.left;
        }

        public void DisConnect()
        {
            if (!IsConnected)
            {
                return;
            }

            GameObject.Destroy(mInstance.gameObject);
            mInstance = null;
        }

    }

}
