using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NW
{
    [Serializable]
    public class FacePuzzleData
    {
        public byte Width;
        public byte[] MapData;
        public List<ScriptableObject> _cores = new();
        private readonly List<ICore> _coresChace = new();
        public List<ICore> Cores
        {
            get
            {
                if (_coresChace.Count != _cores.Count)
                {
                    _cores = _cores.Where(x => x is ICore).ToList();
                    _coresChace.Clear();
                    foreach (var item in _cores)
                    {
                        var instance = ScriptableObject.Instantiate(item);
                        _coresChace.Add(instance as ICore);
                    }
                }
                return _coresChace;
            }
        }
        public List<ScriptableObject> _instances = new();

        private readonly List<IInstance> _instancesChace = new();
        public List<IInstance> Instances
        {
            get
            {
                if (_instances.Count != _instancesChace.Count)
                {
                    _instances = _instances.Where(x => x is IInstance).ToList();
                    _instancesChace.Clear();
                    foreach (var item in _instances)
                    {
                        var instance = ScriptableObject.Instantiate(item);
                        _instancesChace.Add(instance as IInstance);
                    }
                }
                return _instancesChace;
            }
        }
    }
}
