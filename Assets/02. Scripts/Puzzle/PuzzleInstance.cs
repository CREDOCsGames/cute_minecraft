using UnityEngine;
using Util;

namespace Puzzle
{
    public interface IPuzzleInstance
    {
        public Mediator Madiator { get; set; }
        public void InstreamData(byte[] data);
        public byte Width { get; }
        public MatrixBool PuzzleMap { get; }
    }

    public abstract class PuzzleInstance<T> : ScriptableObject, IPuzzleInstance where T : MonoBehaviour
    {
        public Mediator Madiator { get; set; }

        public abstract byte Width { get; }

        public abstract MatrixBool PuzzleMap { get; }

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
                Debug.Log($"Ω«∆– : {data}");
            }
            else
            {
                var elements = _cubeMap.GetElements(data[0], data[1], data[2]);
                _presentation.UpstreamData(elements, data[3]);
            }
        }

        private void OutStreamData(byte[] data)
        {
            Madiator.DownstramData(data);
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

        private void Awake()
        {
            Instantiate(out _cubeMap);
            SetDataLink(out _dataLink);
            LinkCubeElements();
            _dataLink.OnInteraction += OutStreamData;
            SetPresentation(out _presentation);

            var parent = new GameObject("∆€¡Ò").transform;
            foreach (var piece in _cubeMap.Elements)
            {
                piece.transform.parent = parent;
            }
        }

    }
}

