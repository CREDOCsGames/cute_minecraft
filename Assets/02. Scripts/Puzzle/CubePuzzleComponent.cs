using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Puzzle
{
    public class CubePuzzleComponent : MonoBehaviour
    {
        private MessageObserverFromInstance _systemMessageFromInstance;
        private MessageObserverFromCore _systemMessageFromCore;
        private CubeMap<byte> _cubeMap;
        private CubePuzzleReader _puzzleReader;
        private CubePuzzleReaderForCore _puzzleReaderForCore;
        private readonly Mediator _mediatorCenter = new();
        private readonly UnityEvent _onReady = new();
        private readonly UnityEvent<Face> _onClearLevel = new();
        private readonly UnityEvent<Face> _onRotate = new();
        private readonly UnityEvent<Face> _onStartLevel = new();
        [SerializeField] private CubePuzzleData _cubePuzzleData;

        #region portfolio part1
        private void Awake()
        {
            if (_cubePuzzleData.Faces.Length != 6)
            {
                Debug.LogWarning($"Check Cube Puzzle Data. Length : {_cubePuzzleData.Faces.Length}");
                return;
            }

            _cubeMap = new(_cubePuzzleData.Width, _cubePuzzleData.Elements);
            _onReady.AddListener(StartNextPuzzle);

            _systemMessageFromCore = new MessageObserverFromCore(new SystemReader());
            _systemMessageFromCore.OnRecieveMessage += MoveNextLevel;

            _systemMessageFromInstance = new MessageObserverFromInstance(new SystemReader());
            _systemMessageFromInstance.OnRecieveMessage += OnReady;
            _systemMessageFromInstance.OnRecieveMessage += OnRotateCube;

            _puzzleReader = new(_cubePuzzleData, _onRotate);
            _puzzleReader.CoreObservers.Add(_systemMessageFromInstance);
            _puzzleReader.InstanceObservers.Add(_systemMessageFromCore);

            _puzzleReaderForCore = new(_cubeMap,
                                       _onStartLevel,
                                       _onClearLevel,
                                       _onRotate,
                                       _onReady);

            SettingAllResources();
            StartPuzzle(Face.top);
        }
        #endregion
        #region portfolio part2
        private void SettingAllResources()
        {
            _puzzleReader.ReadAllCores(out List<ICore> inUseCores);
            MediatorBinder.BindEvent(inUseCores, _mediatorCenter);
            _mediatorCenter.SetCores(inUseCores);

            _puzzleReader.ReadAllInstances(out List<IInstance> inUseInstances);
            MediatorBinder.BindEvent(inUseInstances, _mediatorCenter);
            _mediatorCenter.SetInstances(inUseInstances);

            inUseCores.ForEach(x => (x as IPuzzleCore)?.Init(_puzzleReaderForCore));
            inUseInstances.ForEach(x => (x as IPuzzleInstance)?.Init(_puzzleReader));
        }
        private void ClearAllResources()
        {
            _puzzleReader.ReadAllCores(out var usedCores);
            MediatorBinder.UnbindEvent(usedCores, _mediatorCenter);
            usedCores.ForEach(x => (x as IDestroyable)?.Destroy());

            _puzzleReader.ReadAllInstances(out var usedInstances);
            MediatorBinder.UnbindEvent(usedInstances, _mediatorCenter);
            usedInstances.ForEach(x => (x as IDestroyable)?.Destroy());
        }
        #endregion
        #region portfolio part3
        private void SettingNewlyUsedResources(Face nextFace)
        {
            _puzzleReader.ReadDifferenceOfSets(nextFace, _puzzleReader.CurrentFace, out List<ICore> newlyCores);
            MediatorBinder.BindEvent(newlyCores, _mediatorCenter);

            _puzzleReader.ReadAllCores(nextFace, out var inUseCores);
            _mediatorCenter.SetCores(inUseCores);

            _puzzleReader.ReadDifferenceOfSets(nextFace, _puzzleReader.CurrentFace, out List<IInstance> newlyInstances);
            MediatorBinder.BindEvent(newlyInstances, _mediatorCenter);

            _puzzleReader.ReadAllInstances(nextFace, out var inUseInstances);
            _mediatorCenter.SetInstances(inUseInstances);

            newlyCores.ForEach(x => (x as IPuzzleCore)?.Init(_puzzleReaderForCore));
            newlyInstances.ForEach(x => (x as IPuzzleInstance)?.Init(_puzzleReader));
        }
        private void ClearUnusedResources(Face nextFace)
        {
            _puzzleReader.ReadDifferenceOfSets(_puzzleReader.CurrentFace, nextFace, out List<ICore> usedCores);
            MediatorBinder.UnbindEvent(usedCores, _mediatorCenter);
            usedCores.ForEach(x => (x as IDestroyable)?.Destroy());

            _puzzleReader.ReadDifferenceOfSets(_puzzleReader.CurrentFace, nextFace, out List<IInstance> usedInstances);
            MediatorBinder.UnbindEvent(usedInstances, _mediatorCenter);
            usedInstances.ForEach(x => (x as IDestroyable)?.Destroy());
        }
        #endregion
        #region portfolio part4
        private void StartPuzzle(Face playFace)
        {
            if (!Enum.IsDefined(typeof(Face), playFace))
            {
                Debug.LogWarning($"Failed to start : {playFace} face");
                return;
            }
            ClearUnusedResources(playFace);
            SettingNewlyUsedResources(playFace);
            _puzzleReader.MoveReadWindow(playFace);
            _onStartLevel.Invoke(playFace);
        }
        private void StartNextPuzzle()
        {
            StartPuzzle(_puzzleReader.CurrentFace + 1);
        }
        private void MoveNextLevel(byte[] data)
        {
            if (!SystemReader.IsClearFace(data))
            {
                return;
            }
            _onClearLevel?.Invoke((Face)(data[0] - 1));

            if (SystemReader.CLEAR_BOTTOM_FACE.Equals(data))
            {
                ClearAllResources();
            }
        }
        #endregion
        #region portfolio part5
        private void OnRotateCube(byte[] data)
        {
            if (!data.Equals(SystemReader.ROTATE_CUBE))
            {
                return;
            }

            const float threshold = 0.98f;
            var up = _puzzleReader.BaseTransform.up;
            var right = _puzzleReader.BaseTransform.right;
            var forward = _puzzleReader.BaseTransform.forward;

            var playFace = Vector3.Dot(up, Vector3.up) > threshold ? Face.top :
                           Vector3.Dot(-up, Vector3.up) > threshold ? Face.bottom :
                           Vector3.Dot(right, Vector3.up) > threshold ? Face.right :
                           Vector3.Dot(-right, Vector3.up) > threshold ? Face.left :
                           Vector3.Dot(forward, Vector3.up) > threshold ? Face.front :
                           Vector3.Dot(-forward, Vector3.up) > threshold ? Face.back : Face.top;

            _onRotate.Invoke(playFace);
        }
        private void OnReady(byte[] data)
        {
            if (!data.Equals(SystemReader.READY_PLAYER))
            {
                return;
            }
            _onReady.Invoke();
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, _cubePuzzleData.BaseTransformSize.extents);
        }
        private void OnDestroy()
        {
            ClearAllResources();
            _onReady.RemoveAllListeners();
            _systemMessageFromCore.OnRecieveMessage -= MoveNextLevel;
            _systemMessageFromInstance.OnRecieveMessage -= OnReady;
            _systemMessageFromInstance.OnRecieveMessage -= OnRotateCube;
        }
    }
    #endregion
}