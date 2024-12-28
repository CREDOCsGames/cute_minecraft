using PlatformGame.Debugger;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    public class CubePuzzleComponent : MonoBehaviour
    {
        private MessageObserver _systemMessageObserver;
        private MessageObserver _puzzleMessageObserver;
        private MessageObserver _monsterMessageObserver;
        private MediatorCenter _mediatorCenter;
        private CubePuzzleDataReader _puzzleDataReader;
        private CubeMap<byte> _cubeMap;
        private CubeMapReader _cubeMapReader;
        [SerializeField] private CubePuzzleData _cubePuzzleData;

        private SlimeSpawner _spawnerCore;

        private void Awake()
        {
            if (_cubePuzzleData.Faces.Length != 6)
            {
                return;
            }

            _puzzleDataReader = new CubePuzzleDataReader(_cubePuzzleData);
            _mediatorCenter = new MediatorCenter();
            _systemMessageObserver = new MessageObserver(new SystemReader());
            _systemMessageObserver.RecieveSystemMessage += MoveNextFace;
            _systemMessageObserver.RecieveSystemMessage += PrintSystemMessage;
            _puzzleMessageObserver = new MessageObserver(new FlowerReader());
            _puzzleMessageObserver.RecieveSystemMessage += PrintFlowerMessage;
            _monsterMessageObserver = new MessageObserver(new MonsterReader());
            _monsterMessageObserver.RecieveSystemMessage += PrintMonsterMessage;
            _cubeMap = new(_cubePuzzleData.Width, _cubePuzzleData.Elements);
            _cubeMapReader = new CubeMapReader(_cubePuzzleData);
            _spawnerCore = new SlimeSpawner();

            SettingAllResources();
            StartPuzzle(Face.top);
        }
        private void StartPuzzle(Face playFace)
        {
            CleanUnusedResources(playFace);
            SettingNewlyUsedResources(playFace);
            _puzzleDataReader.MoveReadWindow(playFace);

            _puzzleDataReader.ReadAllCores(out var inUseCores);
            inUseCores.Add(_spawnerCore); //
            _mediatorCenter.SetCores(inUseCores);

            _puzzleDataReader.ReadAllInstances(out var inUseInstances);
            inUseInstances.Add(_systemMessageObserver);  //
            inUseInstances.Add(_puzzleMessageObserver);  //
            inUseInstances.Add(_monsterMessageObserver); //
            _mediatorCenter.SetInstances(inUseInstances);
        }
        private void CleanAllResources()
        {
            _puzzleDataReader.ReadAllCores(out var usedCores);
            usedCores.Add(_spawnerCore);//
            _puzzleDataReader.ReadAllInstances(out var usedInstances);
            EventBinder.UnbindEvent(usedCores, _mediatorCenter);
            EventBinder.UnbindEvent(usedInstances, _mediatorCenter);
            usedCores.ForEach(x => (x as IDestroyable)?.Destroy());
            usedInstances.ForEach(x => (x as IDestroyable)?.Destroy());
        }
        private void CleanUnusedResources(Face nextFace)
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
            inUseCores.Add(_spawnerCore);//
            inUseCores.ForEach(x => (x as IPuzzleCore)?.Init(_cubeMap));
            inUseInstances.ForEach(x => (x as IPuzzleInstance)?.Init(_cubeMapReader));
            EventBinder.BindEvent(inUseCores, _mediatorCenter);
            EventBinder.BindEvent(inUseInstances, _mediatorCenter);
        }
        private void SettingNewlyUsedResources(Face nextFace)
        {
            _puzzleDataReader.ReadIntersection(_puzzleDataReader.ReadWindow, nextFace, out List<ICore> inUseCores);
            _puzzleDataReader.ReadIntersection(_puzzleDataReader.ReadWindow, nextFace, out List<IInstance> inUseInstances);
            EventBinder.BindEvent(inUseCores, _mediatorCenter);
            EventBinder.BindEvent(inUseInstances, _mediatorCenter);
            inUseCores.ForEach(x => (x as IPuzzleCore)?.Init(_cubeMap));
            inUseInstances.ForEach(x => (x as IPuzzleInstance)?.Init(_cubeMapReader));
        }
        private void MoveNextFace(byte[] data)
        {
            if (SystemReader.CLEAR_BOTTOM_FACE.Equals(data))
            {
                CleanAllResources();
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
        private void Update()
        {
            _spawnerCore.Update();
        }

    }
}