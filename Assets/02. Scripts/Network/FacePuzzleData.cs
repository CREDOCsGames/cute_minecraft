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
        public List<ICore> Cores => _cores.Where(x => x is ICore).Select(x => x as ICore).ToList();
        public List<ScriptableObject> _instances = new();
        public List<IInstance> Instances => _instances.Where(x => x is IInstance).Select(x => x as IInstance).ToList();
    }
}
