using UnityEngine;

namespace Puzzle
{
    public static class PuzzleFactory
    {
        public static IPuzzleCore CreateCoreAs(IPuzzleInstance instance, Madiator madiator, CubeMap<byte> cubeMap)
        {
            if (instance is PuzzleInstance<Flower>)
            {
                return new FlowerPuzzleCore(madiator,cubeMap);
            }
            Debug.Assert(false, "");
            return null;
        }
    }
}
