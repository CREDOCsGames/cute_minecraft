using Flow;
using System;
using UnityEngine;

namespace Puzzle
{
    public class SlimeSpawner : MonoBehaviour, ICore, IPuzzleCore, IReleasable
    {
        public DataReader DataReader { get; private set; } = new MonsterReader();
        private IMediatorCore _mediator;
        private byte[] _spawnMessage;
        private readonly Timer _spawnTimer = new Timer();
        private CubeMap<byte> _map;
        private Face _currentLevel;
        private Face _playingFace;
        private float _exitTime;
        private bool _bWasSpawn;
        [SerializeField] private float _killTime = 1f;
        [SerializeField, Range(0, 1000)] private float _interval = 15f;
        private CubePuzzleReaderForCore _reader;

        public void Awake()
        {
            _spawnTimer.OnTimeout += (t) => SendSpawnMessage();
        }
        public void InstreamData(byte[] data)
        {
            if (data.Equals(MonsterReader.BOSS_SPIT_OUT_FAIL))
            {
                _bWasSpawn = false;
            }
        }
        public void SetMediator(IMediatorCore mediator)
        {
            _mediator = mediator;
        }
        private void Update()
        {
            if (_interval != _spawnTimer.Timeout)
            {
                _spawnTimer.SetTimeout(_interval);
            }
            _spawnTimer.DoTick();
        }

        private void FixedUpdate()
        {
            if (_mediator == null)
            {
                return;
            }
            if (_bWasSpawn && _exitTime < Time.time)
            {
                _bWasSpawn = false;
                var x = _spawnMessage[0];
                var y = _spawnMessage[1];
                var z = (byte)_playingFace;
                if (z == (byte)Face.bottom || _map.GetElements(x, y, z) != 0)
                {
                    return;
                }
                _map.SetElements(x, y, z, (byte)UnityEngine.Random.Range(1, 3));
                var flower = _map.GetElements(x, y, z);
                _mediator.InstreamDataCore<FlowerReader>(new byte[] { x, y, z, flower });
                _mediator.InstreamDataCore<MonsterReader>(MonsterReader.BOSS_SPIT_OUT_SUCCESS);
            }
            if (_bWasSpawn)
            {
                return;
            }
            if (!_spawnTimer.IsStart)
            {
                _spawnTimer.DoStart();
            }
        }
        private void SendSpawnMessage()
        {
            if (_currentLevel is Face.bottom)
            {
                if (TryCalculateSpanwPosition(out var position))
                {
                    MonsterReader.CreateSlimeSpawnData(position[0], position[1], out _spawnMessage);
                    _mediator.InstreamDataCore<MonsterReader>(_spawnMessage);
                    _exitTime = Time.time + _killTime;
                    _bWasSpawn = true;
                }
            }
            else
            {
                _spawnMessage = MonsterReader.SLIME_SPAWN;
                _mediator.InstreamDataCore<MonsterReader>(_spawnMessage);
            }
        }
        public void Init(CubePuzzleReaderForCore reader)
        {
            _map = reader.Map;
            _reader = reader;
            _spawnTimer.DoStart();
            _spawnTimer.DoPause();
            _reader.PuzzleEvent.OnClearLevel += OnClearLevel;
            _reader.PuzzleEvent.OnStartLevel += OnStartLevel;
            _reader.PuzzleEvent.OnRotated += OnRotatedCube;
        }
        public void DoRelease()
        {
            _reader.PuzzleEvent.OnRotated -= OnRotatedCube;
            _reader.PuzzleEvent.OnStartLevel -= OnStartLevel;
            _reader.PuzzleEvent.OnClearLevel -= OnClearLevel;
            _spawnTimer.DoStop();
        }
        private bool TryCalculateSpanwPosition(out byte[] position)
        {
            var indices = _map.GetIndex(_playingFace);
            indices.Sort((x, y) => UnityEngine.Random.Range(-1, 2));
            foreach (var index in indices)
            {
                if (_map.GetElements(index[0], index[1], index[2]) == 0)
                {
                    position = new byte[] { index[0], index[1] };
                    return true;
                }
            }

            position = new byte[] { };
            return false;
        }
        private void OnStartLevel(Face face)
        {
            _currentLevel = face;
            _spawnTimer.DoStop();
            _spawnTimer.DoStart();
        }
        private void OnClearLevel(Face face)
        {
            _spawnTimer.DoPause();
        }
        private void OnRotatedCube(Face preFace, Face playFace)
        {
            _playingFace = playFace;
        }
    }
}
