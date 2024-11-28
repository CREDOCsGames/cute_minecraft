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

        public void AddInstance(IInstance puzzle, TunnelFlag flag)
        {
            foreach (TunnelFlag value in Enum.GetValues(typeof(TunnelFlag)))
            {
                if ((flag & value) != value)
                {
                    continue;
                }
                _tunnelMap.TryAdd(value, new Mediator());
                _tunnelMap[value].AddInstance(puzzle);
            }
        }
        public void AddCore(ICore core, TunnelFlag flag)
        {
            foreach (TunnelFlag bit in Enum.GetValues(typeof(TunnelFlag)))
            {
                if ((flag & bit) != bit)
                {
                    continue;
                }
                _tunnelMap.TryAdd(bit, new Mediator());
                _tunnelMap[bit].AddCore(core);
            }
        }

    }
}
