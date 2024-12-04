using UnityEngine;
using static Puzzle.MediatorCenter;

namespace Puzzle
{
    public static class PuzzleFactory
    {
        public static ICore CreateCoreAs(TunnelFlag flag, Mediator mediator, CubeMap<byte> cubeMap)
        {
            if (flag.HasFlag(TunnelFlag.Flower))
            {
                return new FlowerPuzzleCore(mediator, cubeMap);
            }
            Debug.Assert(false, "");
            return null;
        }
    }
}
