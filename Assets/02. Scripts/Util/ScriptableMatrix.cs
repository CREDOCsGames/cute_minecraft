using System;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    [Serializable]
    public class CustomList<T>
    {
        public List<T> List = new();
    }

    public class ScriptableDictionary<K, V> : ScriptableObject
    {
        readonly List<K> Keys = new();
        readonly List<V> Values = new();

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