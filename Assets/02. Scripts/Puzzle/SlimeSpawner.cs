using Flow;
using System;
using UnityEngine;

namespace Puzzle
{
    public class SlimeSpawner : MonoBehaviour, ICore, IPuzzleCore, IDestroyable
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
        [SerializeField] private FaceFlags _bossStages;
        [SerializeField, Range(0, 1000)] private float _interval = 15f;
        private CubePuzzleReaderForCore _reader;


        public void Awake()
        {
            _spawnTimer.OnTimeoutEvent += (t) => SendSpawnMessage();
            _spawnTimer.OnTimeoutEvent += (t) => _spawnTimer.Start();
        }
        public void InstreamData(byte[] data)
        {
            if(data.Equals(MonsterReader.BOSS_SPIT_OUT_FAIL))
            {
                _bWasSpawn = false;
            }
        }
        public void SetMediator(IMediatorCore mediator)
        {
            _mediator = mediator;
            _spawnTimer.Start();
        }
        private void Update()
        {
            if (_interval != _spawnTimer.Timeout)
            {
                _spawnTimer.SetTimeout(_interval);
            }
            _spawnTimer.Tick();
        }

        private void FixedUpdate()
        {
            if (_bWasSpawn && _exitTime < Time.time)
            {
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
                _bWasSpawn = false;
                _spawnTimer.Start();
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
                    _spawnTimer.Stop();
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
            reader.OnChangedStage += OnChangedStage;
            reader.OnRotatedStage += OnRotatedCube;
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

        private void OnChangedStage(Face face)
        {
            _currentLevel = face;
        }
        private void OnRotatedCube(Face face)
        {
            _playingFace = face;
        }

        public void Destroy()
        {
            _reader.OnChangedStage -= OnChangedStage;
        }
    }
}
