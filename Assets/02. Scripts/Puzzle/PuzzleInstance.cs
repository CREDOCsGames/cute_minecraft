using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Puzzle
{
    public interface IPuzzleInstance
    {
        public Madiator Madiator { get; set; }
        public void InstreamData(byte[] data);
    }

    public abstract class PuzzleInstance<T> : ScriptableObject, IPuzzleInstance where T : MonoBehaviour
    {
        public Madiator Madiator { get; set; }
        CubeMap<T> mCubeMap;
        IDataLink<T> mDataLink;
        IPresentation<T> mPresentation;

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
                var elements = mCubeMap.GetElements(data[0], data[1], data[2]);
                mPresentation.InstreamData(elements, data[3]);
            }
        }

        void OutStreamData(byte[] data)
        {
            Madiator.DownstramData(data);
        }

        void LinkCubeElements()
        {
            foreach (var index in mCubeMap.GetIndex())
            {
                mDataLink.Link(
                mCubeMap.GetElements(index[0], index[1], index[2]),
                new[] { index[0], index[1], index[2], (byte)0 }
                );
            }

        }

        private void Awake()
        {
            Instantiate(out mCubeMap);
            SetDataLink(out mDataLink);
            LinkCubeElements();
            mDataLink.OnInteraction += OutStreamData;
            SetPresentation(out mPresentation);

            var parent = new GameObject("∆€¡Ò").transform;
            foreach (var piece in mCubeMap.Elements)
            {
                piece.transform.parent = parent;
            }
        }

    }
}

