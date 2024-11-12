using System;

namespace Puzzle
{
    public abstract class PuzzleCore : IPuzzleCore
    {
        CubeMap<byte> mCubeMap;
        public CubeMap<byte> CubeMap { get => mCubeMap; set => mCubeMap = value; }
        public Madiator Madiator { get; private set; }

        public PuzzleCore(Madiator madiator, CubeMap<byte> map)
        {
            Madiator = madiator;
            CubeMap = map;
        }

        public void InstramData(byte[] data)
        {
            var input = Vector4Byte.Convert2Vector4Byte(data);

            EditData(in input, out var outputs, in mCubeMap);
            foreach (var output in outputs)
            {
                Madiator.UpstramData(Vector4Byte.Convert2ByteArr(output));
            }

        }

        protected abstract void EditData(in Vector4Byte input, out Vector4Byte[] output, in CubeMap<byte> cubeMap);

        public void Init()
        {
            for (byte face = 0; face < 6; face++)
            {
                for (byte y = 0; y < mCubeMap.Width; y++)
                {
                    for (byte x = 0; x < mCubeMap.Width; x++)
                    {
                        Madiator.UpstramData(new[] { x, y, face, mCubeMap.GetElements(x, y, face) });
                    }
                }
            }
        }
    }

}
