using System;
using UnityEngine;

namespace Puzzle
{
    public abstract class PuzzleCoreContainer : ICore
    {
        private readonly PuzzleCore _core;
        public event Action<byte[]> InstreamEvent;

        public PuzzleCoreContainer(PuzzleCore core)
        {
            _core = core;
        }

        public PuzzleCoreContainer(PuzzleCoreContainer container)
        {
            Debug.Assert(container._core != null);
            _core = container._core;
        }

        public void InstreamData(byte[] data)
        {
            _core.InstreamData(data);
            EditData(data, out var outputs, _core.CubeMap);
            foreach (var output in outputs)
            {
                InstreamEvent.Invoke(Vector4Byte.Convert2ByteArr(output));
            }
        }
        protected abstract void EditData(byte[] data, out Vector4Byte[] output, in CubeMap<byte> cubeMap);
    }

    public abstract class PuzzleCore : ICore
    {
        public event Action<byte[]> InstreamEvent;
        private CubeMap<byte> _cubeMap;
        public CubeMap<byte> CubeMap { get => _cubeMap; set => _cubeMap = value; }
        public Mediator Mediator { get; private set; }

        public PuzzleCore(Mediator mediator, CubeMap<byte> map)
        {
            Mediator = mediator;
            CubeMap = map;
        }

        public void InstreamData(byte[] data)
        {
            var input = Vector4Byte.Convert2Vector4Byte(data);

            EditData(in input, out var outputs, in _cubeMap);
            foreach (var output in outputs)
            {
                InstreamEvent.Invoke(Vector4Byte.Convert2ByteArr(output));
            }

        }

        protected abstract void EditData(in Vector4Byte input, out Vector4Byte[] output, in CubeMap<byte> cubeMap);

        public void Init()
        {
            foreach (var index in _cubeMap.GetIndex())
            {
                InstreamEvent.Invoke(new[]
                {
                    index[0],
                    index[1],
                    index[2],
                    _cubeMap.GetElements(index[0], index[1], index[2])
                });
            }
        }

    }
}
