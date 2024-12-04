using System;
using System.Collections.Generic;
using System.Linq;
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

    [Serializable]
    public class Matrix<T>
    {
        private readonly List<List<T>> _matrix = new();
        private readonly T _default;
        public int RowsCount => _matrix.Count;
        public int ColumnsCount { get; private set; }
        public bool IsEmpty => RowsCount == 0;
        public int ElementsCount => RowsCount * ColumnsCount;

        public Matrix(T @default)
        {
            _default = @default;
            _matrix = new();
        }

        public void SetMatrix(List<T> data, int columnCount)
        {
            ColumnsCount = columnCount;
            _matrix.Clear();
            for (int i = 0; i < data.Count; i++)
            {
                if (i % columnCount == 0)
                {
                    _matrix.Add(new List<T>());
                }
                _matrix.Last().Add(data[i]);
            }
        }

        public List<T> GetElements()
        {
            var list = new List<T>();
            foreach (var item in _matrix)
            {
                list.AddRange(item);
            }

            return list;
        }

        public bool TryGetElement(int row, int column, out T value)
        {
            value = _matrix[row][column];
            return !IsOutOfRange(row, column);
        }


        public void SetElement(int row, int column, T value)
        {
            if (IsOutOfRange(row, column))
            {
                return;
            }
            _matrix[row][column] = value;
        }


        public void AddRank()
        {

            AddRows();
            if (RowsCount == 1)
            {
                return;
            }
            AddColumns();
        }

        public void SubtractRank()
        {
            RemoveRows();
            RemoveColumns();
        }

        private void AddRows()
        {
            _matrix.Add(new());

            if (ColumnsCount == 0)
            {
                ColumnsCount++;
            }

            for (int i = 0; i < ColumnsCount; i++)
            {
                _matrix.Last().Add(_default);
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
                item.Add(_default);
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
                item.RemoveAt(ColumnsCount - 1);
            }
            ColumnsCount--;
        }

        private bool IsOutOfRange(int row, int column)
        {
            Debug.Assert(-1 < row && -1 < column, $"out of range row : {row} or column : {column}");
            Debug.Assert(row < RowsCount && column < ColumnsCount, $"out of range row : {row} or column : {column}");

            return row < 0 || RowsCount <= row ||
                column < 0 || ColumnsCount <= column;
        }

    }


    public class ScriptableMatrix<T> : ScriptableObject
    {
        [SerializeField] List<CustomList<T>> mMatrix = new();
        public List<CustomList<T>> Matrix => mMatrix;
    }
}