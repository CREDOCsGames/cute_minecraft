using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Puzzle
{
    public class CubeMapReader
    {
        private readonly CubeMap<byte> _cubeMap;
        public byte Width => _cubeMap.Width;
        public readonly Transform BaseTransform;
        public readonly Vector3 BaseTransformSize;
        public CubeMapReader(CubePuzzleData puzzleData)
        {
            _cubeMap = new CubeMap<byte>(puzzleData.Width, puzzleData.Elements);
            BaseTransform = puzzleData.BaseTransform;
            BaseTransformSize = puzzleData.BaseTransformSize.extents;
        }

        public bool TryGetElements(out byte[] elements)
        {
            elements = _cubeMap.Elements.ToArray();
            return true;
        }
        public List<byte[]> GetIndex()
            => _cubeMap.GetIndex();
        public byte GetElement(byte x, byte y, byte z)
        {

            return _cubeMap.GetElements(x, y, z);
        }
        public List<byte> GetFace(byte face) => _cubeMap.GetFace((Puzzle.Face)face);


    }
}
