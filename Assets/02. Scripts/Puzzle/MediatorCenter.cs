using System;
using System.Collections.Generic;

namespace Puzzle
{

    public class MediatorCenter
    {
        [Flags]
        public enum TunnelFlag
        {
            None = 0,
            Flower = 1 << 0,
            System = 1 << 1
        }

        private readonly Dictionary<TunnelFlag, Mediator> _tunnelMap = new();
        public List<ICore> Cores = new();
        public List<IInstance> Instances = new();
        public void AddInstance(IInstance puzzle, TunnelFlag flag)
        {
            foreach (TunnelFlag value in Enum.GetValues(typeof(TunnelFlag)))
            {
                if (!flag.HasFlag(value) || value == TunnelFlag.None)
                {
                    continue;
                }
                _tunnelMap.TryAdd(value, new Mediator());
                _tunnelMap[value].AddInstance(puzzle);
            }
            Instances.Add(puzzle);
        }
        public void AddCore(CubeMap<byte> map, TunnelFlag flag)
        {
            foreach (TunnelFlag bit in Enum.GetValues(typeof(TunnelFlag)))
            {
                if (!flag.HasFlag(bit) || bit == TunnelFlag.None)
                {
                    continue;
                }
                _tunnelMap.TryAdd(bit, new Mediator());
                var core = PuzzleFactory.CreateCoreAs(bit, _tunnelMap[bit], map);
                Cores.Add(core);
                _tunnelMap[bit].AddCore(core);
            }


        }

    }
}
