using System;
using UnityEngine;
using Util;

namespace Puzzle
{
    public interface IInstance
    {
        public void InstreamData(byte[] data);
        public event Action<byte[]> InstreamEvent;
    }

    public abstract class PuzzleInstance : ScriptableObject, NW.IInstance
    {
        private NW.IMediatorInstance _mediator;
        public event Action<byte[]> InstreamEvent;
        private IDataLink _dataLink;
        private IPresentation _presentation;
        private NW.DataReader _dataReader;
        public NW.DataReader DataReader => _dataReader;

        protected abstract void Instantiate(PuzzleCubeData puzzleCubeData);
        protected abstract void SetDataLink(out IDataLink dataLink);
        protected abstract void SetPresentation(out IPresentation presentation);
        protected abstract void SetDataReader(out NW.DataReader reader);

        public void InstreamData(byte[] data)
        {
            //if (DataReader.IsReadable(data))
            //{
                _presentation.InstreamData(data);
            //}
        }

        public void Init(PuzzleCubeData puzzleCubeData)
        {
            (this as IDestroyable)?.Destroy();
            Instantiate(puzzleCubeData);
            SetDataLink(out _dataLink);
            SetDataReader(out _dataReader);
            SetPresentation(out _presentation);
        }

        public void SetMediator(NW.IMediatorInstance mediator)
        {
            _dataLink.Mediator = mediator;
        }

    }
}

