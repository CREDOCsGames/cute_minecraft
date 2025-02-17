using System.Collections.Generic;
using System.Linq;

namespace Puzzle
{
    public interface IMediatorCore
    {
        public void InstreamDataCore<T>(byte[] data) where T : DataReader;
    }
    public interface IMediatorInstance
    {
        public void InstreamDataInstance<T>(byte[] data) where T : DataReader;
    }
    public class Mediator : IMediatorCore, IMediatorInstance
    {
        private readonly List<ICore> _cores = new();
        private readonly List<IInstance> _instances = new();

        public void SetCores(List<ICore> cores)
        {
            _cores.Clear();
            _cores.AddRange(cores);
        }

        public void SetInstances(List<IInstance> instances)
        {
            _instances.Clear();
            _instances.AddRange(instances);
        }

        public void InstreamDataCore<T>(byte[] data) where T : DataReader
        {
            _instances.Where(instance => instance.DataReader is T)
                      .ToList()
                      .ForEach(instance => instance.InstreamData(data));
        }
        public void InstreamDataInstance<T>(byte[] data) where T : DataReader
        {
            _cores.Where(core => core.DataReader is T)
                  .ToList()
                  .ForEach(core => core.InstreamData(data));
        }

    }
}