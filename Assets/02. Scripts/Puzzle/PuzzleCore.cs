namespace Puzzle
{
    public abstract class PuzzleCore : IPuzzleCore
    {
        private CubeMap<byte> _cubeMap;
        public CubeMap<byte> CubeMap { get => _cubeMap; set => _cubeMap = value; }
        public Mediator Mediator { get; private set; }

        public PuzzleCore(Mediator mediator, CubeMap<byte> map)
        {
            Mediator = mediator;
            CubeMap = map;
        }

        public void InstramData(byte[] data)
        {
            var input = Vector4Byte.Convert2Vector4Byte(data);

            EditData(in input, out var outputs, in _cubeMap);
            foreach (var output in outputs)
            {
                Mediator.UpstramData(Vector4Byte.Convert2ByteArr(output));
            }

        }

        protected abstract void EditData(in Vector4Byte input, out Vector4Byte[] output, in CubeMap<byte> cubeMap);

        public void Init()
        {
            for (byte face = 0; face < 6; face++)
            {
                for (byte y = 0; y < _cubeMap.Width; y++)
                {
                    for (byte x = 0; x < _cubeMap.Width; x++)
                    {
                        Mediator.UpstramData(new[] { x, y, face, _cubeMap.GetElements(x, y, face) });
                    }
                }
            }
        }
    }

}
