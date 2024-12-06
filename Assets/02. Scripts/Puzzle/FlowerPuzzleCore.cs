using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Puzzle
{
    public class FlowerPuzzleCore : PuzzleCore
    {
        public FlowerPuzzleCore(CubeMap<byte> map) : base(map)
        {
        }

        private bool _clear;
        protected override void EditData(in Vector4Byte input, out Vector4Byte[] output, in CubeMap<byte> cubeMap)
        {
            if (_clear)
            {
                output = new[] { Vector4Byte.FAIL };
                return;
            }

            switch (input.w)
            {
                case 1:
                    Cross(in input, out output, in cubeMap);
                    break;
                case 2:
                    Dot(in input, out output, in cubeMap);
                    break;
                case 3:
                    Create(in input, out output, in cubeMap);
                    break;
                default:
                    output = new[] { Vector4Byte.FAIL };
                    Debug.LogWarning($"{input.w}");
                    return;
            }

            CheckClear(in cubeMap);
        }


        private void CheckClear(in CubeMap<byte> cubeMap)
        {
            var first = cubeMap.Elements.First();
            _clear = cubeMap.Elements.All(x => x == 1 || x == 0) || cubeMap.Elements.All(x => x == 2 || x == 0);
        }

        private static readonly List<int[]> indices = new() { new[] { 0, 0 }, new[] { 1, 0 }, new[] { -1, 0 }, new[] { 0, 1 }, new[] { 0, -1 } };
        private void Cross(in Vector4Byte input, out Vector4Byte[] output, in CubeMap<byte> cubeMap)
        {
            var flower = cubeMap.GetElements(input.x, input.y, input.z);
            Vector4Byte result;
            if (flower != 1 && flower != 2)
            {
                result = Vector4Byte.FAIL;
                output = new[] { result };
                return;
            }

            output = new Vector4Byte[0];
            foreach (var dxdy in indices)
            {
                if (input.x + dxdy[0] < 0 || cubeMap.Width <= input.x + dxdy[0] ||
                    input.y + dxdy[1] < 0 || cubeMap.Width <= input.y + dxdy[1])
                {
                    continue;
                }

                var index = new Vector4Byte((byte)(input.x + dxdy[0]), (byte)(input.y + dxdy[1]), input.z, input.w);
                flower = cubeMap.GetElements(index.x, index.y, index.z);
                if (flower != 1 && flower != 2)
                {
                    continue;
                }
                Vector4Byte[] temp;
                Dot(in index, out temp, cubeMap);
                output = output.Concat(temp).ToArray();
            }

        }

        private void Dot(in Vector4Byte input, out Vector4Byte[] output, in CubeMap<byte> cubeMap)
        {
            var flower = cubeMap.GetElements(input.x, input.y, input.z);
            Vector4Byte result;
            switch (flower)
            {
                case 1:
                    result = new(input.x, input.y, input.z, 2);
                    break;
                case 2:
                    result = new(input.x, input.y, input.z, 1);
                    break;
                default:
                    result = Vector4Byte.FAIL;
                    output = new[] { result };
                    return;
            }
            cubeMap.SetElements(result.x, result.y, result.z, result.w);
            output = new[] { result };
        }

        private void Create(in Vector4Byte input, out Vector4Byte[] output, in CubeMap<byte> cubeMap)
        {
            var flower = cubeMap.GetElements(input.x, input.y, input.z);
            Vector4Byte result;
            switch (flower)
            {
                case 0:
                    result = new(input.x, input.y, input.z, (byte)Random.Range(1, 2));
                    break;
                default:
                    result = Vector4Byte.FAIL;
                    output = new[] { result };
                    return;
            }
            cubeMap.SetElements(result.x, result.y, result.z, result.w);
            output = new[] { result };
        }

    }
}
