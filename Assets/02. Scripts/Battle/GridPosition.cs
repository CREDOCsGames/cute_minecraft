using UnityEngine;

public class GridPosition : MonoBehaviour
{
    private Transform _baseTransform;
    [SerializeField] private bool _useX;
    [SerializeField] private bool _useY;
    [SerializeField] private bool _useZ;

    private void Awake()
    {
        if (transform.root == transform)
        {
            return;
        }
        _baseTransform = new GameObject("가이드라인").transform;
        _baseTransform.parent = transform.parent;
        _baseTransform.localPosition = transform.localPosition;
    }

    private void LateUpdate()
    {
        if (transform.root == transform)
        {
            return;
        }
        MoveItToTheFrontCell(_baseTransform.position, out var cell);
        transform.position = cell;
        Debug.Log(cell);

    }

    [SerializeField] private int _size = 1;
    private void MoveItToTheFrontCell(Vector3 position, out Vector3 cellPosition)
    {
        if (_useX)
        {
            position.x = ((int)position.x) / _size * _size;
        }
        if (_useY)
        {
            position.y = ((int)position.y) / _size * _size;
        }
        if (_useZ)
        {
            position.z = ((int)position.z) / _size * _size;
        }
        cellPosition = position;
    }


    private int _gizmoLength = 10;
    [SerializeField] private Vector3 _offset;
    private int _cubeSize = 1;
    private void OnDrawGizmosSelected()
    {
        for (int r = -_gizmoLength / 2; r <= _gizmoLength / 2; r++)
        {
            for (int c = -_gizmoLength / 2; c <= _gizmoLength / 2; c++)
            {
                var center = _offset;
                center.x = c;
                center.z = r;
                var cubeSize = Vector3.one * _cubeSize;
                cubeSize.y = 0.1f;

                MoveItToTheFrontCell(transform.position, out var myPos);
                if (myPos.x == c && myPos.z == r)
                {
                    Gizmos.color = Color.green;
                }
                Gizmos.DrawWireCube(center, cubeSize);
                Gizmos.color = Color.white;
            }
        }
    }

}
