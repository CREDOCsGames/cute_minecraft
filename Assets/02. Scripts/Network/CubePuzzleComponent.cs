using UnityEngine;

namespace NW
{
    public class CubePuzzleComponent : MonoBehaviour
    {
        [SerializeField] private CubePuzzleData _cubePuzzleData;
        private MediatorCenter _mediatorCenter;

        private void Awake()
        {
            if (_cubePuzzleData.Faces.Length != 6)
            {
                return;
            }
            _mediatorCenter = new MediatorCenter(_cubePuzzleData);
            _mediatorCenter.StartPuzzle(Face.Top);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, _cubePuzzleData.CubeSize.extents);
        }

    }
}