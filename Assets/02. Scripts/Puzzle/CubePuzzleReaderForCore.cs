using System;
using UnityEngine.Events;

namespace Puzzle
{
    public class CubePuzzleReaderForCore
    {
        public readonly CubeMap<byte> Map;
        public event Action<Face> OnChangedStage;
        public event Action<Face> OnRotatedStage;

        public CubePuzzleReaderForCore(
            CubeMap<byte> map,
            UnityEvent<Face> onChangedStage,
            UnityEvent<Face> onRotatedStage)
        {
            Map = map;
            onChangedStage.AddListener((face) => OnChangedStage?.Invoke(face));
            onRotatedStage.AddListener((face) => OnRotatedStage?.Invoke(face));
        }
    }

}
