using System;
using System.Linq;
using NW;
using UnityEngine;
using static Puzzle.Flower;
using Random = UnityEngine.Random;

namespace Puzzle
{
    //public abstract class PuzzleCoreContainer : ICore
    //{
    //    private readonly PuzzleCore _core;
    //    public event Action<byte[]> InstreamEvent;

    //    public PuzzleCoreContainer(PuzzleCore core)
    //    {
    //        core.InstreamEvent += InstreamEvent.Invoke;
    //    }

    //    public PuzzleCoreContainer(PuzzleCoreContainer container)
    //    {
    //        Debug.Assert(container._core != null);
    //        _core = container._core;
    //        _core.InstreamEvent += InstreamEvent.Invoke;
    //    }

    //    public void InstreamData(byte[] data)
    //    {
    //        _core.InstreamData(data);
    //        EditData(data, out var outputs, _core.CubeMap);
    //        foreach (var output in outputs)
    //        {
    //            InstreamEvent.Invoke(Vector4Byte.Convert2ByteArr(output));
    //        }
    //    }
    //    protected abstract void EditData(byte[] data, out Vector4Byte[] output, in CubeMap<byte> cubeMap);

    //    public void Init()
    //    {
    //        _core.Init();
    //    }
    //}

    //public class SystemCore : ICore
    //{
    //    private PuzzleCore _core;
    //    public SystemCore(PuzzleCore core)
    //    {
    //        _core = core;
    //        _core.InstreamEvent += OutstreamData;
    //    }

    //    public event Action<byte[]> InstreamEvent;
    //    private Face _currentFace;

    //    public void InstreamData(byte[] data)
    //    {
    //        if (data[2] != (byte)_currentFace)
    //        {
    //            return;
    //        }
    //        _core.InstreamData(data);
    //        var list = _core.CubeMap.GetFace(_currentFace).Where(x => x == (byte)FlowerType.Green || x == (byte)FlowerType.Red);
    //        var common = list.First();
    //        if (list.All(x => x.Equals(common)))
    //        {
    //            InstreamEvent?.Invoke(SystemMessage.CLEAR_FACE[(int)_currentFace++]);
    //        }

    //        // TODO BOSS
    //        if (data[2] == (byte)Face.bottom)
    //        {
    //            int i = Random.Range(0, 5);
    //            if (i == 0)
    //            {
    //                var indices = _core.CubeMap
    //                    .GetIndex()
    //                    .Where(x => x[2] == (byte)_currentFace)
    //                    .Where(x => _core.CubeMap.GetElements(x[0], x[1], x[2]) == 0)
    //                    .ToList();
    //                var pos = indices[Random.Range(0, indices.Count)];
    //                byte type = 0;
    //                InstreamEvent?.Invoke(new[] { pos[0], pos[1], type });
    //            }
    //        }
    //        //
    //    }

    //    private void OutstreamData(byte[] data)
    //    {
    //        InstreamEvent?.Invoke(data);
    //    }
    //}

    public abstract class PuzzleCore : ScriptableObject, NW.ICore
    {
        protected abstract CubeMap<byte> CubeMap { get; set; }

        public abstract NW.DataReader DataReader { get; }
        protected abstract IMediatorCore _mediator { get; set; }

        public void Init(CubeMap<byte> map)
        {
            CubeMap = map;
        }

        public abstract void InstreamData(byte[] data);

        public void SetMediator(NW.IMediatorCore mediator)
        {
            _mediator = mediator;
        }
    }
}
