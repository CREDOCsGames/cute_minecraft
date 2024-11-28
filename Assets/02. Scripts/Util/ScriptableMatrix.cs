using System;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    [Serializable]
    public class CustomList<T>
    {
        public List<T> List = new();
        public CustomList()
        {

        }

        public CustomList(int capacity, T value)
        {
            List = new();
            for (int i = 0; i < capacity; i++)
            {
                List.Add(value);
            }
        }

        public CustomList(List<T> collection)
        {
            List = collection;
        }
    }

    public class ScriptableDictionary<K, V> : ScriptableObject
    {
        private readonly List<K> Keys = new();
        private readonly List<V> Values = new();

        public bool TryGetValue(K key, out V value)
        {
            Debug.Assert(Keys.Count == Values.Count, $"Keys and values do not match. : {name}");
            var index = Keys.IndexOf(key);
            value = Values[index];
            return -1 < index;
        }
    }

    public class Matrix<T>
    {
        [SerializeField] List<CustomList<T>> _matrix = new();
        private readonly T _default;
        public int RowsCount => _matrix.Count;
        public int ColumnsCount { get; private set; }

        public Matrix(int capacity, T @default)
        {
            _default = @default;
            _matrix = new(capacity);
            for (int i = 0; i < capacity; i++)
            {
                _matrix.Add(new CustomList<T>());
            }
        }

        public void AddRank()
        {
            AddRows();
            AddColumns();
        }

        public void SubtractRank()
        {
            RemoveRows();
            RemoveColumns();
        }

        private void AddRows()
        {
            _matrix.Add(new CustomList<T>());

            if (ColumnsCount == 0)
            {
                ColumnsCount++;
            }

            for (int i = 0; i < ColumnsCount; i++)
            {
                _matrix[^0].List.Add(_default);
            }
        }
        private void RemoveRows()
        {
            if (RowsCount == 0)
            {
                return;
            }
            _matrix.RemoveAt(RowsCount - 1);
        }
        private void AddColumns()
        {
            if (RowsCount == 0)
            {
                AddRows();
                return;
            }

            foreach (var item in _matrix)
            {
                item.List.Add(_default);
            }

            ColumnsCount++;
        }
        private void RemoveColumns()
        {
            if (ColumnsCount == 0)
            {
                return;
            }

            foreach (var item in _matrix)
            {
                item.List.RemoveAt(ColumnsCount - 1);
            }
        }
    }


    public class ScriptableMatrix<T> : ScriptableObject
    {
        [SerializeField] List<CustomList<T>> mMatrix = new();
        public List<CustomList<T>> Matrix => mMatrix;
    }
}