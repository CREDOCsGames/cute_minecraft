using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Puzzle
{
    [Serializable]
    public class CubePuzzleData
    {
        public Transform BaseTransform;
        public Bounds BaseTransformSize;
        public byte Width;
        public byte[] Elements => Faces.SelectMany(x => x.MapData).ToArray();
        public List<UnityEngine.Object> _globalCores = new();
        private readonly List<ICore> _globalCoresChace = new();
        public List<ICore> GlobalCores
        {
            get
            {
                if (_globalCores.Count != _globalCoresChace.Count)
                {
                    _globalCores = _globalCores.Where(x => x is ICore).ToList();
                    _globalCoresChace.Clear();
                    foreach (var core in _globalCores)
                    {
                        if (core is ScriptableObject)
                        {
                            var instance = ScriptableObject.Instantiate(core);
                            _globalCoresChace.Add(instance as ICore);
                        }
                        else
                        {
                            _globalCoresChace.Add(core as ICore);
                        }
                    }
                }
                return _globalCoresChace;
            }
        }
        public List<UnityEngine.Object> _globalInstances = new();
        private readonly List<IInstance> _globalInstancesChace = new();
        public List<IInstance> GlobalInstances
        {
            get
            {
                if (_globalInstances.Count != _globalInstancesChace.Count)
                {
                    _globalInstances = _globalInstances.Where(x => x is IInstance).ToList();
                    _globalInstancesChace.Clear();
                    foreach (var item in _globalInstances)
                    {
                        if (item is ScriptableObject)
                        {
                            var instance = ScriptableObject.Instantiate(item);
                            _globalInstancesChace.Add(instance as IInstance);
                        }
                        else
                        {
                            _globalInstancesChace.Add(item as IInstance);
                        }
                    }
                }
                return _globalInstancesChace;
            }
        }
        public FacePuzzleData[] Faces;
    }

}
