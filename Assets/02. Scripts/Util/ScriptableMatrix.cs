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

    public class ScriptableMatrix<T> : ScriptableObject
    {
        [SerializeField] List<CustomList<T>> mMatrix = new();
        public List<CustomList<T>> Matrix => mMatrix;
    }
}