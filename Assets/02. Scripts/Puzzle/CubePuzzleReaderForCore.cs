using System;
using UnityEngine.Events;

namespace Puzzle
{
    public class CubePuzzleReaderForCore
    {
        public readonly CubeMap<byte> Map;
        public event Action OnReady;
        public event Action<Face> OnStartLevel;
        public event Action<Face> OnClearLevel;
        public event Action<Face> OnRotatedCube;
        public CubePuzzleReaderForCore(
            CubeMap<byte> map,
            UnityEvent<Face> onStartLevel,
            UnityEvent<Face> onClearLevel,
            UnityEvent<Face> onRotatedCube,
            UnityEvent onReady)
        {
            Map = map;
            onStartLevel.AddListener((face) => OnStartLevel?.Invoke(face));
            onClearLevel.AddListener((face) => OnClearLevel?.Invoke(face));
            onRotatedCube.AddListener((face) => OnRotatedCube?.Invoke(face));
            onReady.AddListener(() => OnReady?.Invoke());
        }
    }

}
