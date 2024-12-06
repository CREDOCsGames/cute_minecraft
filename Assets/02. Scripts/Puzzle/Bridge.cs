using UnityEngine;

namespace Puzzle
{
    public class Bridge
    {
        public bool IsConnected => _instance;
        public bool IsConnectable => Vector3.Distance(_pointA, _pointB) <= _limit && Vector3.Distance(_pointA, _pointB) > 5;
        private Vector3 _pointA { get; set; }
        private Vector3 _pointB { get; set; }
        private GameObject _instance;
        private readonly float _limit;
        private static readonly Material _LED = Resources.Load<Material>("Materials/_LED_Bridge");

        private static readonly Vector3[] _directions =
        {
            new Vector3(1, 1, 0),
            new Vector3(0, 1, -1),
            new Vector3(-1, 1, 0),
            new Vector3(0, 1, 1)
        };

        public Bridge(float limit)
        {
            _limit = limit;
        }

        public void SetConnectionPoints(Bounds basis, Bounds area)
        {
            basis.extents /= 2;
            area.extents /= 2;

            var line = new Vector2(area.center.x, area.center.z) - new Vector2(basis.center.x, basis.center.z);
            var angle = Mathf.Atan2(line.x, line.y) * Mathf.Rad2Deg - 45;
            var indexBasis = (int)Mathf.Repeat(angle, 360f) / 90;
            var indexArea = (int)Mathf.Repeat(indexBasis + 2, 4);

            _pointA = basis.center + Vector3.Scale(_directions[indexBasis], basis.extents);
            _pointB = area.center + Vector3.Scale(_directions[indexArea], area.extents);
        }

        public void ConnectAtoB()
        {
            if (!IsConnectable)
            {
                return;
            }

            _instance = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _instance.GetComponent<MeshRenderer>().material = _LED;
            _instance.layer = LayerMask.NameToLayer("Bridge");

            var size = new Vector3(1.5f, 0.5f, Vector3.Distance(_pointA, _pointB) + 1);
            _instance.transform.localScale = size;

            var center = (_pointA + _pointB) / 2 - Vector3.up * size.y / 2;
            _instance.transform.position = center;

            _instance.transform.LookAt(_pointB);

            CerateColliderWall();
        }

        private void CerateColliderWall()
        {
            var obj = new GameObject();
            obj.transform.SetParent(_instance.transform);

            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.up * Mathf.Abs(_pointA.y - _pointB.y) + Vector3.right + Vector3.forward;
            obj.transform.localEulerAngles = Vector3.left * Mathf.Abs(obj.transform.parent.eulerAngles.x);

            var scale = new Vector3(1 / 1.5f * 0.1f,
                (2 / _instance.transform.lossyScale.y) * Mathf.Abs(_pointA.y - _pointB.y), 1);

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

            Object.Destroy(_instance);
            _instance = null;
        }
    }
}