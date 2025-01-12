using PlatformGame.Debugger;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Puzzle
{
    public class CubePuzzleComponent : MonoBehaviour
    {
        private MessageObserver _systemMessageObserver;
        private MessageObserver _puzzleMessageObserver;
        private MessageObserver _monsterMessageObserver;
        private MediatorCenter _mediatorCenter;
        private CubeMap<byte> _cubeMap;
        private CubePuzzleDataReader _puzzleReaderForInstance;
        private CubePuzzleDataReader _puzzleDataReader;
        private CubePuzzleReaderForCore _puzzleReaderForCore;
        private readonly UnityEvent<Face> _rotatedStageEvent = new();
        private readonly UnityEvent<Face> _changedStageEvent = new();
        [SerializeField] private CubePuzzleData _cubePuzzleData;

        private void Awake()
        {
            if (_cubePuzzleData.Faces.Length != 6)
            {
                return;
            }

            _mediatorCenter = new MediatorCenter();

            _systemMessageObserver = new MessageObserver(new SystemReader());
            _systemMessageObserver.RecieveSystemMessage += MoveNextFace;
            _systemMessageObserver.RecieveSystemMessage += PrintSystemMessage;

            _puzzleMessageObserver = new MessageObserver(new FlowerReader());
            _puzzleMessageObserver.RecieveSystemMessage += PrintFlowerMessage;

            _monsterMessageObserver = new MessageObserver(new MonsterReader());
            _monsterMessageObserver.RecieveSystemMessage += PrintMonsterMessage;

            _puzzleDataReader = new CubePuzzleDataReader(_cubePuzzleData);
            _puzzleDataReader.GlobalInstanceObservers.Add(_systemMessageObserver);
            _puzzleDataReader.GlobalInstanceObservers.Add(_puzzleMessageObserver);
            _puzzleDataReader.GlobalInstanceObservers.Add(_monsterMessageObserver);

            _cubeMap = new(_cubePuzzleData.Width, _cubePuzzleData.Elements);
            _puzzleReaderForInstance = new CubePuzzleDataReader(_cubePuzzleData);
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
            _puzzleDataReader.ReadAllInstances(out var usedInstances);
            EventBinder.UnbindEvent(usedCores, _mediatorCenter);
            EventBinder.UnbindEvent(usedInstances, _mediatorCenter);
            usedCores.ForEach(x => (x as IDestroyable)?.Destroy());
            usedInstances.ForEach(x => (x as IDestroyable)?.Destroy());
        }
        private void ClearUnusedResources(Face nextFace)
        {
            _puzzleDataReader.ReadDifferenceOfSets(_puzzleDataReader.ReadWindow, nextFace, out List<ICore> usedCores);
            _puzzleDataReader.ReadDifferenceOfSets(_puzzleDataReader.ReadWindow, nextFace, out List<IInstance> usedInstances);
            EventBinder.UnbindEvent(usedCores, _mediatorCenter);
            EventBinder.UnbindEvent(usedInstances, _mediatorCenter);
            usedCores.ForEach(x => (x as IDestroyable)?.Destroy());
            usedInstances.ForEach(x => (x as IDestroyable)?.Destroy());
        }
        private void SettingAllResources()
        {
            _puzzleDataReader.ReadAllCores(out List<ICore> inUseCores);
            _puzzleDataReader.ReadAllInstances(out List<IInstance> inUseInstances);
            _mediatorCenter.SetCores(inUseCores);
            EventBinder.BindEvent(inUseCores, _mediatorCenter);
            _mediatorCenter.SetInstances(inUseInstances);
            EventBinder.BindEvent(inUseInstances, _mediatorCenter);
            inUseCores.ForEach(x => (x as IPuzzleCore)?.Init(_puzzleReaderForCore));
            inUseInstances.ForEach(x => (x as IPuzzleInstance)?.Init(_puzzleReaderForInstance));
        }
        private void SettingNewlyUsedResources(Face nextFace)
        {
            _puzzleDataReader.ReadIntersection(_puzzleDataReader.ReadWindow, nextFace, out List<ICore> newlyCores);
            _puzzleDataReader.ReadAllCores(nextFace, out var inUseCores);
            _mediatorCenter.SetCores(inUseCores);
            EventBinder.BindEvent(newlyCores, _mediatorCenter);

            _puzzleDataReader.ReadIntersection(_puzzleDataReader.ReadWindow, nextFace, out List<IInstance> newlyInstances);
            _puzzleDataReader.ReadAllInstances(nextFace, out var inUseInstances);
            _mediatorCenter.SetInstances(inUseInstances);
            EventBinder.BindEvent(newlyInstances, _mediatorCenter);

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