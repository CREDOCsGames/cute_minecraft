using System.Collections.Generic;
using System.Linq;

namespace NW
{
    public static class PuzzleDataReaderUtil
    {
        public static void ReadAllInstance(CubePuzzleData puzzleData, Face face, out List<IInstance> instances)
        {
            instances = new List<IInstance>();
            instances.AddRange(puzzleData.GlobalInstances);
            instances.AddRange(puzzleData.Faces[(int)face].Instances);
        }
        public static void ReadDifferenceOfSets(CubePuzzleData puzzleData, Face A, Face B, out List<IInstance> differenceOfSets)
        {
            differenceOfSets = null;
        }
        public static void ReadIntersection(CubePuzzleData puzzleData, Face A, Face B, out List<IInstance> intersection)
        {
            intersection = null;
        }
        public static void ReadAllCore(CubePuzzleData puzzleData, Face face, out List<ICore> cores)
        {
            cores = new List<ICore>();
            cores.AddRange(puzzleData.GlobalCores);
            cores.AddRange(puzzleData.Faces[(int)face].Cores);
        }
        public static void ReadDifferenceOfSets(CubePuzzleData puzzleData, Face A, Face B, out List<ICore> differenceOfSets)
        {
            differenceOfSets = null;
        }
        public static void ReadIntersection(CubePuzzleData puzzleData, Face A, Face B, out List<ICore> intersection)
        {
            intersection = null;
        }
    }

}