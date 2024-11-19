using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Puzzle
{
    public interface IPuzzleInstance
    {
        public Madiator Madiator { get; set; }
        public void InstreamData(byte[] data);
        public byte Width { get; }
        public MatrixBool PuzzleMap { get; }
    }

    public abstract class PuzzleInstance<T> : ScriptableObject, IPuzzleInstance where T : MonoBehaviour
    {
        public Madiator Madiator { get; set; }

        public abstract byte Width { get; }

        public abstract MatrixBool PuzzleMap { get; }

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
                Debug.Log($"½ÇÆÐ : {data}");
            }
            else
            {
                var elements = mCubeMap.GetElements(data[0], data[1], data[2]);
                mPresentation.UpstreamData(elements, data[3]);
            }
        }

        void OutStreamData(byte[] data)
        {
            Madiator.DownstramData(data);
        }

        void LinkCubeElements()
        {
            //foreach (var index in mCubeMap.GetIndex())
            //{

            //}
            for (byte face = 0; face < 6; face++)
            {
                for (byte y = 0; y < Width; y++)
                {
                    for (byte x = 0; x < Width; x++)
                    {
                        mDataLink.Link(
                            mCubeMap.GetElements(x, y, face),
                            new[] { x, y, face, (byte)0 }
                            );
                    }
                }
            }
        }

        void Awake()
        {
            Instantiate(out mCubeMap);
            SetDataLink(out mDataLink);
            LinkCubeElements();
            mDataLink.OnInteraction += OutStreamData;
            SetPresentation(out mPresentation);

            var parent = new GameObject("ÆÛÁñ").transform;
            foreach (var piece in mCubeMap.Elements)
            {
                piece.transform.parent = parent;
            }
        }

    }
}

