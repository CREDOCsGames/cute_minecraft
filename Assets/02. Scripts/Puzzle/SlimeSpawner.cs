using Flow;

namespace Puzzle
{
    public class SlimeSpawner : ICore
    {
        public DataReader DataReader { get; private set; } = new SystemReader();
        private float _interval = 15f;
        private IMediatorCore _mediator;
        private Timer _spawnTimer = new Timer();
        private byte[] _spawnMessage = MonsterReader.SLIME_SPAWN;

        public SlimeSpawner()
        {
            _spawnTimer.SetTimeout(_interval);
            _spawnTimer.OnTimeoutEvent += (t) => _mediator.InstreamDataCore<MonsterReader>(_spawnMessage);
            _spawnTimer.OnTimeoutEvent += (t) => _spawnTimer.Start();
        }
        public void InstreamData(byte[] data)
        {
            if (SystemReader.CLEAR_BACK_FACE.Equals(data))
            {
                _spawnMessage = MonsterReader.BOSS_SPIT_OUT;
            }
        }
        public void SetMediator(IMediatorCore mediator)
        {
            _mediator = mediator;
            _spawnTimer.Start();
        }
        public void Update()
        {
            _spawnTimer.Tick();
        }

    }
}
