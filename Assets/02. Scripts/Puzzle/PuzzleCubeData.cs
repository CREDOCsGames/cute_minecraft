using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Puzzle
{
    [Serializable]
    public readonly struct PuzzleCubeData
    {
        public readonly Transform Base;
        public readonly List<byte> Map;
        public readonly List<ICore> Cores;
        public readonly List<IInstance> Instances;
        public readonly PuzzleFaceData[] Faces;
        public readonly byte Width;

        public static DataReader GetReader(IInstance instance)
        {
            if (instance.GetType().Equals(typeof(FlowerPuzzleInstance)))
            {
                //return new FlowerReader();
            }
            return new FailReader();
        }
        public static DataReader GetReader(ICore core) 
        {
            if (core.GetType().Equals(typeof(FlowerPuzzleCore)))
            {
                //return new FlowerReader();
            }
            return new FailReader();
        }
    }

    [Serializable]
    public readonly struct PuzzleFaceData
    {
        public readonly List<FlowerPuzzleCore> Cores;
        public readonly List<ScriptableObject> Instances;
    }

}