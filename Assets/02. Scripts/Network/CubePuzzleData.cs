using Puzzle;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NW
{
    [Serializable]
    public class CubePuzzleData
    {
        public Transform BaseTransform;
        public Bounds BaseTransformSize;
        public byte Width;
        public byte[] Elements => Faces.SelectMany(x => x.MapData).ToArray();
        public List<ScriptableObject> _globalCores = new();
        private readonly List<ICore> _globalCoresChace = new();
        public List<ICore> GlobalCores
        {
            get
            {
                if (_globalCores.Count != _globalCoresChace.Count)
                {
                    _globalCores = _globalCores.Where(x => x is PuzzleCore).ToList();
                    _globalCoresChace.Clear();
                    foreach (var core in _globalCores)
                    {
                        var instance = ScriptableObject.Instantiate(core);
                        _globalCoresChace.Add(instance as ICore);
                    }
                }
                return _globalCoresChace;
            }
        }
        public List<ScriptableObject> _globalInstances = new();
        private readonly List<IInstance> _globalInstancesChace = new();
        public List<IInstance> GlobalInstances
        {
            get
            {
                if (_globalInstances.Count != _globalInstancesChace.Count)
                {
                    _globalInstances = _globalInstances.Where(x => x is PuzzleInstance).ToList();
                    _globalInstancesChace.Clear();
                    foreach (var item in _globalInstances)
                    {
                        var instance = ScriptableObject.Instantiate(item);
                        _globalInstancesChace.Add(instance as IInstance);
                    }
                }
                return _globalInstancesChace;
            }
        }
        public FacePuzzleData[] Faces;
    }

}
