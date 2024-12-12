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

    public abstract class PuzzleInstance : ScriptableObject, IInstance
    {
        public Mediator Mediator { get; set; }
        public event Action<byte[]> InstreamEvent;
        private IDataLink _dataLink;
        private IPresentation _presentation;
        private DataReader _dataReader;

        protected abstract void Instantiate(PuzzleCubeData puzzleCubeData);
        protected abstract void SetDataLink(out IDataLink dataLink);
        protected abstract void SetPresentation(out IPresentation presentation);
        protected abstract void SetDataReader(out DataReader reader);

        public void InstreamData(byte[] data)
        {
            if (_dataReader.IsReadable(data))
            {
                _presentation.InstreamData(data);
            }
        }

        public void Init(PuzzleCubeData puzzleCubeData)
        {
            (this as IDestroyable)?.Destroy();
            Instantiate(puzzleCubeData);
            SetDataLink(out _dataLink);
            SetDataReader(out _dataReader);
            SetPresentation(out _presentation);
            _dataLink.OnInteraction += InstreamEvent.Invoke;
        }

    }
}

