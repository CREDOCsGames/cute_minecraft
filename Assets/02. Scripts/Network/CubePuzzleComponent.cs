using PlatformGame.Debugger;
using Puzzle;
using UnityEngine;
using Util;

namespace NW
{
    public class CubePuzzleComponent : MonoBehaviour
    {
        private MessageObserver _systemMessageObserver;
        private MessageObserver _puzzleMessageObserver;
        private MessageObserver _monsterMessageObserver;
        private MediatorCenter _mediatorCenter;
        private CubePuzzleDataReader _puzzleDataReader;
        private CubeMap<byte> _cubeMap;
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

            // TODO
            _cubeMap = new(_cubePuzzleData.Width, _cubePuzzleData.Elements);
            _puzzleDataReader.ReadAllCores(out var cores);
            cores.ForEach(x => (x as PuzzleCore)?.Init(_cubeMap));
            _puzzleDataReader.ReadAllInstances(out var instances);
            var reader = new CubeMapReader(_cubePuzzleData);
            instances.ForEach(x => (x as PuzzleInstance)?.Init(reader));

            // TODO 2
            _spawnerCore = new SlimeSpawner();


            StartPuzzle(Face.Top);
        }
        private void StartPuzzle(Face playFace)
        {
            CleanUnusedResources();
            SettingNewlyUsedResources(playFace);

            _puzzleDataReader.ReadAllCores(out var inUseCores);
            inUseCores.Add(_spawnerCore); //
            _mediatorCenter.SetCores(inUseCores);

            _puzzleDataReader.ReadAllInstances(out var inUseInstances);
            inUseInstances.Add(_systemMessageObserver);  //
            inUseInstances.Add(_puzzleMessageObserver);  //
            inUseInstances.Add(_monsterMessageObserver); //
            _mediatorCenter.SetInstances(inUseInstances);
        }
        private void CleanUnusedResources()
        {
            _puzzleDataReader.ReadAllCores(out var usedCores);
            usedCores.Add(_spawnerCore);//
            _puzzleDataReader.ReadAllInstances(out var usedInstances);
            EventBinder.UnbindEvent(usedCores, _mediatorCenter);
            EventBinder.UnbindEvent(usedInstances, _mediatorCenter);
        }
        private void SettingNewlyUsedResources(Face nextFace)
        {
            _puzzleDataReader.MoveReadWindow(nextFace);
            _puzzleDataReader.ReadAllCores(out var inUseCores);
            inUseCores.Add(_spawnerCore);//
            _puzzleDataReader.ReadAllInstances(out var inUseInstances);
            EventBinder.BindEvent(inUseCores, _mediatorCenter);
            EventBinder.BindEvent(inUseInstances, _mediatorCenter);
        }
        private void MoveNextFace(byte[] data)
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