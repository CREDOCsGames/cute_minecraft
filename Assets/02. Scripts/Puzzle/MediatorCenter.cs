using System;
using System.Collections.Generic;
using Util;

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
            foreach (var tunnel in _tunnelMap)
            {
                if (tunnel.Key.HasFlag(TunnelFlag.System))
                {
                    tunnel.Value.AddInstance(instance);
                }
            }
        }

        readonly private List<IInstance> _Instances = new();
        readonly private List<ICore> _Cores = new();

        public void HubCoreToInstance<T>(byte[] data) where T : DataReader
        {
            foreach (var instance in _Instances)
            {
                if (typeof(T).Equals(PuzzleCubeData.GetReader(instance)))
                {
                    instance.InstreamData(data);
                }
            }
        }
        public void HubInstanceToCore<T>(byte[] data) where T :DataReader
        {
            foreach (var core in _Cores)
            {
                if (typeof(T).Equals(PuzzleCubeData.GetReader(core)))
                {
                    core.InstreamData(data);
                }
            }
        }

    }
}
