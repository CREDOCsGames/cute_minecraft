using System.Collections.Generic;

namespace Puzzle
{
    public class Mediator
    {
        public IPuzzleCore Core { get; set; }
        private readonly List<IPuzzleInstance> _instances = new();

        public void AddInstance(IPuzzleInstance instance)
        {
            _instances.Add(instance);
        }

        public void DownstramData(byte[] data)
        {
            Core.InstramData(data);
        }
        public void UpstramData(byte[] data)
        {
            foreach (var instance in _instances)
            {
                instance.InstreamData(data);
            }
        }
    }

}
