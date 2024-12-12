using System.Collections.Generic;

namespace NW
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
            differenceOfSets = null;
        }
        public void ReadIntersection(Face A, Face B, out List<IInstance> intersection)
        {
            intersection = null;
        }
        public void ReadDifferenceOfSets(Face A, Face B, out List<ICore> differenceOfSets)
        {
            differenceOfSets = null;
        }
        public void ReadIntersection(Face A, Face B, out List<ICore> intersection)
        {
            intersection = null;
        }

    }
}