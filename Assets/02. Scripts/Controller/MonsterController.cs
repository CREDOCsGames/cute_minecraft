using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    [RequireComponent(typeof(Animator))]
    public class MonsterController : MonoBehaviour, IInstance, IPuzzleInstance
    {
        public DataReader DataReader { get; private set; } = new MonsterReader();
        private Animator _animator;
        private Face _playingFace;
        private IMediatorInstance _mediator;
        private MonsterState _characterState;
        private BossController _bossController;
        private CubePuzzleDataReader _puzzleData;
        private CubeMap<GameObject> _cubeMap;
        private readonly Queue<byte[]> _commandQueue = new();

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _bossController = GetComponent<BossController>();
            _bossController.OnFailed += () => _mediator.InstreamDataInstance<MonsterReader>(MonsterReader.BOSS_SPIT_OUT_FAIL);
        }
        public void InstreamData(byte[] data)
        {
            if (data == MonsterReader.BOSS_EXIT)
            {
                _commandQueue.Clear();
            }

            if (_characterState is not MonsterState.None &&
                data == MonsterReader.BOSS_SPAWN)
            {
                Debug.LogWarning($"Boss's status is already {_characterState.ToString()}.");
            }

            if (data.Equals(MonsterReader.BOSS_SPIT_OUT_SUCCESS))
            {
                _bossController.Success();
            }

            _commandQueue.Enqueue(data);
        }
        private void ProcessNextCommand()
        {
            var command = _commandQueue.Dequeue();

            if (command == MonsterReader.BOSS_SPAWN &&
                _characterState is MonsterState.None)
            {
                TrasitionState(MonsterState.Enter);
                return;
            }

            if (_characterState is not MonsterState.Enter)
            {
                return;
            }

            if (command == MonsterReader.BOSS_EXIT)
            {
                TrasitionState(MonsterState.Die);
                return;
            }

            if (MonsterReader.BOSS_SPIT_OUT.Equals(command))
            {
                TrasitionState(MonsterState.Action1);
                _puzzleData.GetPositionAndRotation(new[] { command[0], command[1], (byte)Face.top }, out var position, out var rotation);
                var flower = _cubeMap.GetElements(command[0], command[1], (byte)_playingFace);
                _bossController.SlimeSpawnPoint = _puzzleData.BaseTransform.position + position;
                return;
            }
        }
        public void SetState(MonsterState state)
        {
            _characterState = state;
        }
        private void TrasitionState(MonsterState state)
        {
            _characterState = state;
            _animator.SetTrigger(_characterState.ToString());
        }
        private void Update()
        {
            if (_commandQueue.Count == 0)
            {
                return;
            }

            ProcessNextCommand();
        }
        public void SetMediator(IMediatorInstance mediator)
        {
            _mediator = mediator;
        }
        public void Init(CubePuzzleDataReader puzzleData)
        {
            _puzzleData = puzzleData;
            _puzzleData.OnRotatedStage += OnRotatedCube;
            _cubeMap = new CubeMap<GameObject>(puzzleData.Width, new Instantiator<GameObject>(new GameObject()));

            foreach (var index in _cubeMap.GetIndex())
            {
                var flower = _cubeMap.GetElements(index);
                flower.transform.SetParent(puzzleData.BaseTransform);
                puzzleData.GetPositionAndRotation(index, out var position, out var rotation);
                flower.transform.SetLocalPositionAndRotation(position, rotation);
            }
        }
        private void OnRotatedCube(Face newFace)
        {
            _playingFace = newFace;
        }
    }

    public enum MonsterState // 애니메이터 트리거
    {
        None,
        Enter,
        Action1,
        Die
    }
}