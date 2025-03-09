using System;

namespace Puzzle
{
    public class CubePuzzleReaderForCore
    {
        public readonly CubeMap<byte> Map;
        public readonly CubePuzzleEvent PuzzleEvent;
        public CubePuzzleReaderForCore(
            CubeMap<byte> map,
            CubePuzzleEvent puzzleEvent)
        {
            Map = map;
            PuzzleEvent = puzzleEvent;
        }
    }

}
