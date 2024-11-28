using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Puzzle
{
    public class Mediator
    {
        private readonly List<ICore> _cores = new();
        private readonly List<IInstance> _instances = new();

        public void AddInstance(IInstance instance)
        {
            Debug.Assert(0 < _cores.Count, "At least one core is required.");
            if (_cores.Count == 0)
            {
                return;
            }

            _instances.Add(instance);
            instance.InstreamEvent += DownstramData;
        }

        public void RemoveInstance(IInstance instance)
        {
            _instances.Remove(instance);
            instance.InstreamEvent -= DownstramData;
        }

        public void AddCore(ICore core)
        {
            Debug.Assert(0 < _cores.Count, "At least one core is required.");
            if (_cores.Contains(core))
            {
                return;
            }
            _cores.Add(core);
            core.InstreamEvent += UpstramData;
        }

        public void RemoveCore(ICore core)
        {
            _cores.Remove(core);
            if (_cores.Count == 0)
            {
                _instances.ToList().ForEach(instance => RemoveInstance(instance));
            }
            core.InstreamEvent -= UpstramData;
        }

        private void DownstramData(byte[] data)
        {
            _cores.ForEach(core => core.InstreamData(data));
        }

        private void UpstramData(byte[] data)
        {
            _instances.ForEach(instance => instance.InstreamData(data));
        }
    }

}
