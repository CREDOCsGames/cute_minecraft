using System;
using UnityEngine;

namespace Puzzle
{
    public interface IInstance
    {
        public void InstreamData(byte[] data);
        public event Action<byte[]> InstreamEvent;
    }

    public abstract class PuzzleInstance<T> : ScriptableObject, IInstance where T : MonoBehaviour
    {
        public Mediator Mediator { get; set; }
        public event Action<byte[]> InstreamEvent;
        private CubeMap<T> _cubeMap;
        private IDataLink<T> _dataLink;
        private IPresentation<T> _presentation;

        protected abstract void Instantiate(out CubeMap<T> cubeMap);
        protected abstract void SetDataLink(out IDataLink<T> dataLink);
        protected abstract void SetPresentation(out IPresentation<T> presentation);

        public void InstreamData(byte[] data)
        {
            if (Vector4Byte.FAIL.Equals(data))
            {
                Debug.Log($"fail : {data}");
            }
            else
            {
                var elements = _cubeMap.GetElements(data[0], data[1], data[2]);
                _presentation.InstreamData(elements, data[3]);
            }
        }

        private void LinkCubeElements()
        {
            foreach (var index in _cubeMap.GetIndex())
            {
                _dataLink.Link(
                _cubeMap.GetElements(index[0], index[1], index[2]),
                new[] { index[0], index[1], index[2], (byte)0 }
                );
            }

        }

        private void SetParent(Transform cubeMapObject)
        {
            foreach (var flower in _cubeMap.Elements)
            {
                var position = flower.transform.position;
                flower.transform.SetParent(cubeMapObject);
                flower.transform.localPosition = position;
            }
        }

        public void Init(Transform cubeMapObject)
        {
            Instantiate(out _cubeMap);
            SetParent(cubeMapObject);
            SetDataLink(out _dataLink);
            LinkCubeElements();
            _dataLink.OnInteraction += InstreamEvent.Invoke;
            SetPresentation(out _presentation);
        }

    }
}

