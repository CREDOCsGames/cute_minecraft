using Puzzle;
using UnityEngine;

namespace NW
{
    public class CubePuzzleComponent : MonoBehaviour
    {
        private IInstance _systemMessageObserver;
        private MediatorCenter _mediatorCenter;
        private CubePuzzleDataReader _puzzleDataReader;
        private CubeMap<byte> _cubeMap;
        [SerializeField] private CubePuzzleData _cubePuzzleData;

        private void Awake()
        {
            if (_cubePuzzleData.Faces.Length != 6)
            {
                return;
            }
            _puzzleDataReader = new CubePuzzleDataReader(_cubePuzzleData);
            _mediatorCenter = new MediatorCenter();
            _systemMessageObserver = new SystemMessageObserver();
            (_systemMessageObserver as SystemMessageObserver).RecieveSystemMessage += NextFace;

            // TODO
            _cubeMap = new(_cubePuzzleData.Width, _cubePuzzleData.Elements);
            _puzzleDataReader.ReadAllCores(out var cores);
            cores.ForEach(x => (x as PuzzleCore)?.Init(_cubeMap));
            _puzzleDataReader.ReadAllInstances(out var instances);
            var reader = new CubeMapReader(_cubePuzzleData);
            instances.ForEach(x => (x as PuzzleInstance).Init(reader));

            StartPuzzle(Face.Top);
        }
        private void StartPuzzle(Face playFace)
        {
            CleanUnusedResources();
            SettingNewlyUsedResources(playFace);
            _puzzleDataReader.ReadAllCores(out var inUseCores);
            _mediatorCenter.SetCores(inUseCores);
            _puzzleDataReader.ReadAllInstances(out var inUseInstances);
            _mediatorCenter.SetInstances(inUseInstances);
        }
        private void CleanUnusedResources()
        {
            _puzzleDataReader.ReadAllCores(out var usedCores);
            _puzzleDataReader.ReadAllInstances(out var usedInstances);
            usedInstances.Add(_systemMessageObserver);
            EventBinder.UnbindEvent(usedCores, _mediatorCenter);
            EventBinder.UnbindEvent(usedInstances, _mediatorCenter);
        }
        private void SettingNewlyUsedResources(Face nextFace)
        {
            _puzzleDataReader.MoveReadWindow(nextFace);
            _puzzleDataReader.ReadAllCores(out var inUseCores);
            _puzzleDataReader.ReadAllInstances(out var inUseInstances);
            inUseInstances.Add(_systemMessageObserver);
            EventBinder.BindEvent(inUseCores, _mediatorCenter);
            EventBinder.BindEvent(inUseInstances, _mediatorCenter);
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, _cubePuzzleData.BaseTransformSize.extents);
        }
        // TODO : check run time
        private void NextFace(byte[] data)
        {
            if (SystemReader.CLEAR_BOTTOM_FACE.Equals(data))
            {
                CleanUnusedResources();
            }
            else
            if (SystemReader.IsClearFace(data))
            {
                StartPuzzle(_puzzleDataReader.ReadWindow + 1);
            }
        }

    }
}