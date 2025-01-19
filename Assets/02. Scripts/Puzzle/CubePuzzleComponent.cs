using PlatformGame.Debugger;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Puzzle
{
    public class CubePuzzleComponent : MonoBehaviour
    {
        private MessageObserverCore _systemMessageObserverCore;
        private MessageObserver _systemMessageObserver;
        private MessageObserver _puzzleMessageObserver;
        private MessageObserver _monsterMessageObserver;
        private CubeMap<byte> _cubeMap;
        private CubePuzzleDataReader _puzzleDataReader;
        private CubePuzzleDataReader _puzzleReaderForInstance;
        private CubePuzzleReaderForCore _puzzleReaderForCore;
        private readonly MediatorCenter _mediatorCenter = new();
        private readonly UnityEvent<Face> _rotatedStageEvent = new();
        private readonly UnityEvent<Face> _changedStageEvent = new();
        [SerializeField] private CubePuzzleData _cubePuzzleData;

        private void Awake()
        {
            if (_cubePuzzleData.Faces.Length != 6)
            {
                return;
            }

            _systemMessageObserver = new MessageObserver(new SystemReader());
            _systemMessageObserver.RecieveSystemMessage += MoveNextFace;
            _systemMessageObserver.RecieveSystemMessage += PrintSystemMessage;

            _systemMessageObserverCore = new MessageObserverCore(new SystemReader());
            _systemMessageObserverCore.RecieveSystemMessage += OnRotateFace;

            _puzzleMessageObserver = new MessageObserver(new FlowerReader());
            _puzzleMessageObserver.RecieveSystemMessage += PrintFlowerMessage;

            _monsterMessageObserver = new MessageObserver(new MonsterReader());
            _monsterMessageObserver.RecieveSystemMessage += PrintMonsterMessage;

            _puzzleDataReader = new(_cubePuzzleData, _rotatedStageEvent);
            _puzzleDataReader.GlobalCoreObservers.Add(_systemMessageObserverCore);
            _puzzleDataReader.GlobalInstanceObservers.Add(_systemMessageObserver);
            _puzzleDataReader.GlobalInstanceObservers.Add(_puzzleMessageObserver);
            _puzzleDataReader.GlobalInstanceObservers.Add(_monsterMessageObserver);

            _cubeMap = new(_cubePuzzleData.Width, _cubePuzzleData.Elements);
            _puzzleReaderForInstance = new(_cubePuzzleData, _rotatedStageEvent);
            _puzzleReaderForCore = new(_cubeMap, _changedStageEvent, _rotatedStageEvent);

            SettingAllResources();
            StartPuzzle(Face.top);
        }
        private void StartPuzzle(Face playFace)
        {
            ClearUnusedResources(playFace);
            SettingNewlyUsedResources(playFace);
            _puzzleDataReader.MoveReadWindow(playFace);
            _changedStageEvent.Invoke(playFace);
        }
        private void ClearAllResources()
        {
            _puzzleDataReader.ReadAllCores(out var usedCores);
            EventBinder.UnbindEvent(usedCores, _mediatorCenter);
            usedCores.ForEach(x => (x as IDestroyable)?.Destroy());

            _puzzleDataReader.ReadAllInstances(out var usedInstances);
            EventBinder.UnbindEvent(usedInstances, _mediatorCenter);
            usedInstances.ForEach(x => (x as IDestroyable)?.Destroy());
        }
        private void ClearUnusedResources(Face nextFace)
        {
            _puzzleDataReader.ReadDifferenceOfSets(_puzzleDataReader.ReadWindow, nextFace, out List<ICore> usedCores);
            EventBinder.UnbindEvent(usedCores, _mediatorCenter);
            usedCores.ForEach(x => (x as IDestroyable)?.Destroy());

            _puzzleDataReader.ReadDifferenceOfSets(_puzzleDataReader.ReadWindow, nextFace, out List<IInstance> usedInstances);
            EventBinder.UnbindEvent(usedInstances, _mediatorCenter);
            usedInstances.ForEach(x => (x as IDestroyable)?.Destroy());
        }
        private void SettingAllResources()
        {
            _puzzleDataReader.ReadAllCores(out List<ICore> inUseCores);
            EventBinder.BindEvent(inUseCores, _mediatorCenter);
            _mediatorCenter.SetCores(inUseCores);

            _puzzleDataReader.ReadAllInstances(out List<IInstance> inUseInstances);
            EventBinder.BindEvent(inUseInstances, _mediatorCenter);
            _mediatorCenter.SetInstances(inUseInstances);

            inUseCores.ForEach(x => (x as IPuzzleCore)?.Init(_puzzleReaderForCore));
            inUseInstances.ForEach(x => (x as IPuzzleInstance)?.Init(_puzzleReaderForInstance));
        }
        private void SettingNewlyUsedResources(Face nextFace)
        {
            _puzzleDataReader.ReadIntersection(_puzzleDataReader.ReadWindow, nextFace, out List<ICore> newlyCores);
            EventBinder.BindEvent(newlyCores, _mediatorCenter);

            _puzzleDataReader.ReadAllCores(nextFace, out var inUseCores);
            _mediatorCenter.SetCores(inUseCores);

            _puzzleDataReader.ReadIntersection(_puzzleDataReader.ReadWindow, nextFace, out List<IInstance> newlyInstances);
            EventBinder.BindEvent(newlyInstances, _mediatorCenter);

            _puzzleDataReader.ReadAllInstances(nextFace, out var inUseInstances);
            _mediatorCenter.SetInstances(inUseInstances);

            newlyCores.ForEach(x => (x as IPuzzleCore)?.Init(_puzzleReaderForCore));
            newlyInstances.ForEach(x => (x as IPuzzleInstance)?.Init(_puzzleReaderForInstance));
        }
        private void MoveNextFace(byte[] data)
        {
            if (SystemReader.CLEAR_BOTTOM_FACE.Equals(data))
            {
                ClearAllResources();
            }
            else
            if (SystemReader.IsClearFace(data))
            {
                StartPuzzle(_puzzleDataReader.ReadWindow + 1);
            }
        }
        private void OnRotateFace(byte[] data)
        {
            if (!data.Equals(SystemReader.ROTATE_CUBE))
            {
                return;
            }
            Face target = Face.top;

            var normal = _puzzleDataReader.BaseTransform.up;
            if (normal == Vector3.up)
            {
                target = Face.top;
            }

            normal = -_puzzleDataReader.BaseTransform.up;
            if (normal == Vector3.up)
            {
                target = Face.bottom;
            }

            normal = _puzzleDataReader.BaseTransform.right;
            if (normal == Vector3.up)
            {
                target = Face.right;
            }

            normal = -_puzzleDataReader.BaseTransform.right;
            if (normal == Vector3.up)
            {
                target = Face.left;
            }

            normal = _puzzleDataReader.BaseTransform.forward;
            if (normal == Vector3.up)
            {
                target = Face.front;
            }

            normal = -_puzzleDataReader.BaseTransform.forward;
            if (normal == Vector3.up)
            {
                target = Face.back;
            }

            Debug.Log($"[Rotate] {target}");
            _rotatedStageEvent.Invoke(target);
        }
        private void PrintSystemMessage(byte[] data)
        {
            Debug.Log($"[System] {DebugLog.GetStrings(data)}");
        }
        private void PrintFlowerMessage(byte[] data)
        {
            Debug.Log($"[Puzzle] {DebugLog.GetStrings(data)}");
        }
        private void PrintMonsterMessage(byte[] data)
        {
            Debug.Log($"[Monster] {DebugLog.GetStrings(data)}");
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, _cubePuzzleData.BaseTransformSize.extents);
        }

    }
}