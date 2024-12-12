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
        public Bounds CubeSize;
        public List<ScriptableObject> _globalCores = new();
        public List<ICore> GlobalCores => _globalCores.Where(x => x is ICore).Select(x => x as ICore).ToList();
        public List<ScriptableObject> _globalInstances = new();
        public List<IInstance> GlobalInstances => _globalInstances.Where(x=>x is IInstance).Select(x => x as IInstance).ToList();
        public FacePuzzleData[] Faces;
    }

}
