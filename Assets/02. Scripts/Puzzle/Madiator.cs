using System.Collections.Generic;

namespace Puzzle
{
    public class Madiator
    {
        public IPuzzleCore Core { get; set; }
        readonly List<IPuzzleInstance> mInstances = new();

        public void AddInstance(IPuzzleInstance instance)
        {
            mInstances.Add(instance);
        }

        public void DownstramData(byte[] data)
        {
            Core.InstramData(data);
        }
        public void UpstramData(byte[] data)
        {
            foreach (var instance in mInstances)
            {
                instance.InstreamData(data);
            }
        }
    }

}
