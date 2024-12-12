using NW;
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
        public event Action<byte[]> InstreamEvent;
        private IDataLink _dataLink;
        private IPresentation _presentation;
        private NW.DataReader _dataReader;
        public NW.DataReader DataReader => _dataReader;

        protected abstract void Instantiate(CubeMapReader puzzleData);
        protected abstract void SetDataLink(out IDataLink dataLink);
        protected abstract void SetPresentation(out IPresentation presentation);
        protected abstract void SetDataReader(out NW.DataReader reader);

        public void InstreamData(byte[] data)
        {
            if (DataReader.IsReadable(data))
            {
                _presentation.InstreamData(data);
            }
        }

        public void Init(CubeMapReader puzzleData)
        {
            (this as IDestroyable)?.Destroy();
            SetDataLink(out _dataLink);
            Instantiate(puzzleData);
            SetDataReader(out _dataReader);
            SetPresentation(out _presentation);
            foreach (var ind in puzzleData.GetIndex())
            {
                _presentation.InstreamData(new[] { ind[0], ind[1], ind[2], puzzleData.GetElement(ind[0], ind[1], ind[2]) });
            }
        }

        public void SetMediator(NW.IMediatorInstance mediator)
        {
            _dataLink.Mediator = mediator;
        }

    }
}

