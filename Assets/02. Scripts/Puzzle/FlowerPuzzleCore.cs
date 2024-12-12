using System.Collections.Generic;
using System.Linq;
using NW;
using UnityEngine;
using Util;

namespace Puzzle
{
    public class FlowerPuzzleCore : PuzzleCore
    {
        private readonly NW.DataReader _reader = new FlowerReader();
        public override NW.DataReader DataReader => _reader;

        protected override IMediatorCore _mediator { get; set; }
        protected override CubeMap<byte> CubeMap { get; set; }

        private bool _clear;
        public FlowerPuzzleCore(CubeMap<byte> map) : base(map)
        {
        }
        public override void InstreamData(byte[] data)
        {
            if (_clear)
            {
                return;
            }

            switch (data[3])
            {
                case 1:
                    Cross(data);
                    break;
                case 2:
                    Dot(data);
                    break;
                case 3:
                    Create(data);
                    break;
                default:
                    return;
            }

            CheckClear(CubeMap);
        }


        private void CheckClear(CubeMap<byte> cubeMap)
        {
            var first = cubeMap.Elements.First();
            _clear = cubeMap.Elements.All(x => x == 1 || x == 0) || cubeMap.Elements.All(x => x == 2 || x == 0);
        }

        private static readonly List<int[]> indices = new() { new[] { 0, 0 }, new[] { 1, 0 }, new[] { -1, 0 }, new[] { 0, 1 }, new[] { 0, -1 } };


        private void Cross(byte[] input)
        {
            var flower = CubeMap.GetElements(input[0], input[1], input[2]);
            if (flower != 1 && flower != 2)
            {
                return;
            }

            foreach (var dxdy in indices)
            {
                if (input[0] + dxdy[0] < 0 || CubeMap.Width <= input[0] + dxdy[0] ||
                    input[1] + dxdy[1] < 0 || CubeMap.Width <= input[1] + dxdy[1])
                {
                    continue;
                }

                var index = new byte[] { (byte)(input[0] + dxdy[0]), (byte)(input[1] + dxdy[1]), input[2], input[3] };
                flower = CubeMap.GetElements(index[0], index[1], index[2]);
                if (flower != 1 && flower != 2)
                {
                    continue;
                }
                Dot(index);
            }

        }

        private void Dot(byte[] input)
        {
            var flower = CubeMap.GetElements(input[0], input[1], input[2]);
            byte[] result;
            switch (flower)
            {
                case 1:
                    result = new[] { input[0], input[1], input[2], (byte)2 };
                    break;
                case 2:
                    result = new[] { input[0], input[1], input[2], (byte)1 };
                    break;
                default:
                    return;
            }
            CubeMap.SetElements(result[0], result[1], result[2], result[3]);
            _mediator.InstreamDataCore<FlowerReader>(result);
        }

        private void Create(byte[] input)
        {
            var flower = CubeMap.GetElements(input[0], input[1], input[2]);
            byte[] result;
            switch (flower)
            {
                case 0:
                    result = new[] { input[0], input[1], input[2], (byte)Random.Range(1, 2) };
                    break;
                default:
                    return;
            }
            CubeMap.SetElements(result[0], result[1], result[2], result[3]);
            _mediator.InstreamDataCore<FlowerReader>(result);
        }

    }
}
