using UnityEngine;

namespace Puzzle
{
    public static class PuzzleFactory
    {
        public static IPuzzleCore CreateCoreAs(IPuzzleInstance instance, Mediator mediator, CubeMap<byte> cubeMap)
        {
            if (instance is FlowerPuzzleInstance)
            {
                return new FlowerPuzzleCore(mediator, cubeMap);
            }
            Debug.Assert(false, "");
            return null;
        }
    }
}
