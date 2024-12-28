using System.Collections.Generic;

namespace Puzzle
{
    public class CubePuzzleDataReader
    {
        public Face ReadWindow;
        private readonly CubePuzzleData _cubePuzzleData;

        public CubePuzzleDataReader(CubePuzzleData cubePuzzleData)
        {
            _cubePuzzleData = cubePuzzleData;
        }
        public void MoveReadWindow(Face nextReadWindow)
        {
            ReadWindow = nextReadWindow;
        }
        public void ReadAllCores(out List<ICore> cores)
        {
            cores = new List<ICore>();
            cores.AddRange(_cubePuzzleData.GlobalCores);
            cores.AddRange(_cubePuzzleData.Faces[(int)ReadWindow].Cores);
        }
        public void ReadAllInstances(out List<IInstance> instances)
        {
            instances = new List<IInstance>();
            instances.AddRange(_cubePuzzleData.GlobalInstances);
            instances.AddRange(_cubePuzzleData.Faces[(int)ReadWindow].Instances);
        }
        public void ReadDifferenceOfSets(Face A, Face B, out List<IInstance> differenceOfSets)
        {
            differenceOfSets = new List<IInstance>();
            var setA = _cubePuzzleData.Faces[(byte)A].Instances;
            var setB = _cubePuzzleData.Faces[(byte)B].Instances;
            foreach (var item in setA)
            {
                if (setB.Contains(item))
                {
                    continue;
                }
                differenceOfSets.Add(item);
            }
        }
        public void ReadIntersection(Face A, Face B, out List<IInstance> intersection)
        {
            intersection = new List<IInstance>();
            var setA = _cubePuzzleData.Faces[(byte)A].Instances;
            var setB = _cubePuzzleData.Faces[(byte)B].Instances;
            foreach (var item in setB)
            {
                if (setA.Contains(item))
                {
                    continue;
                }
                intersection.Add(item);
            }
        }
        public void ReadDifferenceOfSets(Face A, Face B, out List<ICore> differenceOfSets)
        {
            differenceOfSets = new List<ICore>();
            var setA = _cubePuzzleData.Faces[(byte)A].Cores;
            var setB = _cubePuzzleData.Faces[(byte)B].Cores;
            foreach (var item in setA)
            {
                if (setB.Contains(item))
                {
                    continue;
                }
                differenceOfSets.Add(item);
            }
        }
        public void ReadIntersection(Face A, Face B, out List<ICore> intersection)
        {
            intersection = new List<ICore>();
            var setA = _cubePuzzleData.Faces[(byte)A].Cores;
            var setB = _cubePuzzleData.Faces[(byte)B].Cores;
            foreach (var item in setB)
            {
                if (setA.Contains(item))
                {
                    continue;
                }
                intersection.Add(item);
            }
        }

    }
}