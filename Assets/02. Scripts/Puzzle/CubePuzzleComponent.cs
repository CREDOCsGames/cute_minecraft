using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Puzzle
{
    public class CubePuzzleComponent : MonoBehaviour
    {
        private CubeMap<byte> _cubeMap;
        private CubePuzzleEvent _puzzleEvent;
        private CubePuzzleReader _puzzleReader;
        private CubePuzzleReaderForCore _coreReader;
        private readonly Mediator _puzzleMediator = new();
        private readonly UnityEvent<Face> _onStartLevel = new();
        [SerializeField] private CubePuzzleData _puzzleData;

        private void Awake()
        {
            if (_puzzleData.Faces.Length != 6)
            {
                Debug.LogWarning($"Check Cube Puzzle Data. Length : {_puzzleData.Faces.Length}");
                return;
            }

            _cubeMap = new(_puzzleData.Width, _puzzleData.Elements);
            _puzzleEvent = new CubePuzzleEvent(_puzzleData.BaseTransform, _onStartLevel);
            _puzzleEvent.OnReady += StartNextPuzzle;
            _puzzleEvent.OnClearStage += EndStage;
            _puzzleData.GlobalCores.Add( _puzzleEvent );
            _puzzleData.GlobalInstances.Add(_puzzleEvent);
            _puzzleReader = new(_puzzleData, _puzzleEvent);
            _coreReader = new(_cubeMap, _puzzleEvent);

            SettingAllResources();
            StartPuzzle(Face.top);
        }
        private void SettingAllResources()
        {
            _puzzleReader.ReadAllCores(out List<ICore> inUseCores);
            MediatorBinder.BindEvent(inUseCores, _puzzleMediator);
            _puzzleMediator.SetCores(inUseCores);

            _puzzleReader.ReadAllInstances(out List<IInstance> inUseInstances);
            MediatorBinder.BindEvent(inUseInstances, _puzzleMediator);
            _puzzleMediator.SetInstances(inUseInstances);

            inUseCores.ForEach(x => (x as IPuzzleCore)?.Init(_coreReader));
            inUseInstances.ForEach(x => (x as IPuzzleInstance)?.InitInstance(_puzzleReader));
        }
        private void ClearAllResources()
        {
            _puzzleReader.ReadAllCores(out var usedCores);
            MediatorBinder.UnbindEvent(usedCores, _puzzleMediator);
            usedCores.ForEach(x => (x as IReleasable)?.DoRelease());

            _puzzleReader.ReadAllInstances(out var usedInstances);
            MediatorBinder.UnbindEvent(usedInstances, _puzzleMediator);
            usedInstances.ForEach(x => (x as IReleasable)?.DoRelease());
        }
        private void SettingNewlyUsedResources(Face nextFace)
        {
            _puzzleReader.ReadDifferenceOfSets(nextFace, _puzzleReader.Stage, out List<ICore> newlyCores);
            MediatorBinder.BindEvent(newlyCores, _puzzleMediator);

            _puzzleReader.ReadAllCores(nextFace, out var inUseCores);
            _puzzleMediator.SetCores(inUseCores);

            _puzzleReader.ReadDifferenceOfSets(nextFace, _puzzleReader.Stage, out List<IInstance> newlyInstances);
            MediatorBinder.BindEvent(newlyInstances, _puzzleMediator);

            _puzzleReader.ReadAllInstances(nextFace, out var inUseInstances);
            _puzzleMediator.SetInstances(inUseInstances);

            newlyCores.ForEach(x => (x as IPuzzleCore)?.Init(_coreReader));
            newlyInstances.ForEach(x => (x as IPuzzleInstance)?.InitInstance(_puzzleReader));
        }
        private void ClearUnusedResources(Face nextFace)
        {
            _puzzleReader.ReadDifferenceOfSets(_puzzleReader.Stage, nextFace, out List<ICore> usedCores);
            MediatorBinder.UnbindEvent(usedCores, _puzzleMediator);
            usedCores.ForEach(x => (x as IReleasable)?.DoRelease());

            _puzzleReader.ReadDifferenceOfSets(_puzzleReader.Stage, nextFace, out List<IInstance> usedInstances);
            MediatorBinder.UnbindEvent(usedInstances, _puzzleMediator);
            usedInstances.ForEach(x => (x as IReleasable)?.DoRelease());
        }
        private void StartPuzzle(Face playFace)
        {
            if (!Enum.IsDefined(typeof(Face), playFace))
            {
                Debug.LogWarning($"Failed to start : {playFace} face");
                return;
            }
            ClearUnusedResources(playFace);
            SettingNewlyUsedResources(playFace);
            _puzzleReader.NextLevel(playFace);
            _onStartLevel.Invoke(playFace);
        }
        private void StartNextPuzzle()
        {
            StartPuzzle(_puzzleReader.Stage + 1);
        }
        private void EndStage()
        {
            ClearAllResources();
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, _puzzleData.BaseTransformSize.extents);
        }
        private void OnDestroy()
        {
            ClearAllResources();
        }
    }
}