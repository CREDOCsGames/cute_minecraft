using System;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    public class CubeMap<T>
    {
        public readonly T[] Elements;
        public readonly byte Width;

        public CubeMap(byte width, ICopyable<T> copier)
        {
            Width = width;
            Elements = new T[(int)Mathf.Pow(width, 2) * Enum.GetValues(typeof(Face)).Length];

            for (int i = 0; i < Elements.Length; i++)
            {
                Elements[i] = copier.Copy();
            }
        }

        public CubeMap(byte width, T origin)
        {
            Width = width;
            Elements = new T[(int)Mathf.Pow(width, 2) * Enum.GetValues(typeof(Face)).Length];

            for (int i = 0; i < Elements.Length; i++)
            {
                Elements[i] = origin;
            }
        }

        public CubeMap(byte width, T[] elements)
        {
            Elements = elements;
            Width = width;
        }

        public T GetElements(byte x, byte y, byte z)
        {
            Debug.Assert(Width * Width * z + Width * y + x < Elements.Length,
                $"Out of range : x {x} y {y} z {z} max {Elements.Length - 1}");
            return Elements[Width * Width * z + Width * y + x];

        }

        public T GetElements(byte[] index)
        {
            Debug.Assert(index.Length == 3);
            var x = index[0];
            var y = index[1];
            var z = index[2];
            return GetElements(x, y, z);
        }

        public void SetElements(byte x, byte y, byte z, T value)
        {
            Debug.Assert(Width * Width * z + Width * y + x < Elements.Length,
    $"Out of range : x {x} y {y} z {z} max {Elements.Length - 1}");
            Elements[Width * Width * z + Width * y + x] = value;
        }

        public List<T> GetFace(Face face)
        {
            List<T> list = new();

            for (byte y = 0; y < Width; y++)
            {
                for (byte x = 0; x < Width; x++)
                {
                    list.Add(GetElements(x, y, (byte)face));
                }
            }

            return list;
        }
        public List<byte[]> GetIndex(Face face)
        {
            List<byte[]> list = new();

            for (byte y = 0; y < Width; y++)
            {
                for (byte x = 0; x < Width; x++)
                {
                    list.Add(new[] { x, y, (byte)face });
                }
            }

            return list;
        }

        public List<byte[]> GetIndex()
        {
            List<byte[]> list = new();
            for (byte face = 0; face < 6; face++)
            {
                for (byte y = 0; y < Width; y++)
                {
                    for (byte x = 0; x < Width; x++)
                    {
                        list.Add(new[] { x, y, face });
                    }
                }
            }
            return list;
        }

    }
}
