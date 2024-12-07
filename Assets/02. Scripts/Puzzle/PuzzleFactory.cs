using UnityEngine;
using static Puzzle.MediatorCenter;

namespace Puzzle
{
    public static class PuzzleFactory
    {
        public static ICore CreateCoreAs(TunnelFlag flag, CubeMap<byte> cubeMap)
        {
            ICore core = null;
            if (flag.HasFlag(TunnelFlag.Flower))
            {
                core = new FlowerPuzzleCore(cubeMap);
            }
            if (flag.HasFlag(TunnelFlag.System) && core is PuzzleCore puzzleCore)
            {
                core = new SystemCore(puzzleCore);
            }

            
            Debug.Assert(core != null, "");
            return core;
        }
    }
}
