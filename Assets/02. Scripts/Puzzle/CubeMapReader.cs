using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;

namespace Puzzle
{
    public class CubeMapReader
    {
        private readonly CubeMap<byte> _cubeMap;
        public byte Width => _cubeMap.Width;
        public CubeMapReader(CubeMap<byte> cubeMap)
        {
            _cubeMap = cubeMap;
        }
        public bool TryGetElements(out byte[] elements)
        {
            elements = _cubeMap.Elements.ToArray();
            return _cubeMap.Elements != null;
        }
        public byte GetElement(byte x, byte y, byte z)
        {
            return _cubeMap.GetElements(x, y, z);
        }
        public byte GetElement(byte[] index)
        {
            Debug.Assert(index.Length == 3);
            var x = index[0];
            var y = index[1];
            var z = index[2];
            return _cubeMap.GetElements(x, y, z);
        }
        public List<byte[]> GetIndex()
        {
            return _cubeMap.GetAllIndex();
        }
        public List<byte> GetFace(byte face)
        {
            return _cubeMap.GetElements((Puzzle.Face)face);
        }
        public bool TryGetCrossIndices(byte x, byte y, byte face, out List<Vector2Int> crossIndices)
        {
            crossIndices = new();
            if (OutOfRange.CheckLine(face, 0, 5))
            {
                return false;
            }

            foreach (var d in Index.CROSS)
            {
                var nx = x + d.x;
                var ny = y + d.y;

                if (OutOfRange.CheckBox(nx, ny, Width, Width))
                {
                    continue;
                }
                crossIndices.Add(new(nx, ny));
            }

            return crossIndices.Count != 0;
        }
    }

}

