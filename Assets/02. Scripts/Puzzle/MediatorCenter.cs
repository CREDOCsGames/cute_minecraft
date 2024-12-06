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
            _tunnelMap.TryAdd(flag, new Mediator());
            _tunnelMap[flag].AddInstance(puzzle);
            Instances.Add(puzzle);
        }
        public void AddCore(CubeMap<byte> map, TunnelFlag flag)
        {
            _tunnelMap.TryAdd(flag, new Mediator());
            var core = PuzzleFactory.CreateCoreAs(flag, map);
            Cores.Add(core);
            _tunnelMap[flag].AddCore(core);
        }

        // TODO REMOVE
        public void AddListenerSystemMessage(IInstance instance)
        {
            foreach(var tunnel in _tunnelMap)
            {
                if (tunnel.Key.HasFlag(TunnelFlag.System))
                {
                    tunnel.Value.AddInstance(instance);
                }
            }
        }

    }
}
