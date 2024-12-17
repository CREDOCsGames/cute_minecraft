using Flow;
using UnityEngine;

namespace NW
{
    public class MonsterReader : DataReader
    {
        public static readonly byte[] BOSS_SPAWN = { 0 };
        public static readonly byte[] BOSS_EXIT = { 1 };
        public static readonly byte[] BOSS_SPIT_OUT = { 2 };
        public static readonly byte[] SLIME_SPAWN = { 3 };
        public static readonly byte[] SLIME_BOUNCE = { 4 };
        public static readonly byte[] SLIME_LAND = { 5 };
        public override bool IsReadable(byte[] data)
        {
            return data.Length == 1;
        }
    }

    public class SlimeSpawner : ICore
    {
        private Timer _spawnTimer = new Timer();
        private IMediatorCore _mediator;
        public DataReader DataReader { get; private set; } = new MonsterReader();

        public void InstreamData(byte[] data)
        {
        }

        public void SetMediator(IMediatorCore mediator)
        {
            _mediator = mediator;
            _spawnTimer.SetTimeout(5f);
        }

        float t;
        bool once = true;
        public void Update()
        {
            if (5f < Time.time - t)
            {
                if (once)
                {
                    _mediator.InstreamDataCore<MonsterReader>(MonsterReader.BOSS_SPAWN);
                    once = false;
                }
                t = Time.time;
                _mediator.InstreamDataCore<MonsterReader>(MonsterReader.BOSS_SPIT_OUT);
            }
        }

    }

}
