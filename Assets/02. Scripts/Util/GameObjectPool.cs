using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace Util
{
    public static class GameObjectPool<T> where T : Object
    {
        private static readonly Dictionary<int, ObjectPool<T>> _particles = new();

        public static ObjectPool<T> GetPool(T origin)
        {
            var key = origin.GetInstanceID();
            _particles.TryAdd(key, new(() => GameObject.Instantiate(origin)));
            return _particles[key];
        }
        public static void ClearAll()
        {
            foreach (var key in _particles.Keys.ToArray())
            {
                ClearPool(key);
            }
        }
        public static void ClearPool(T origin)
        {
            _particles.Remove(origin.GetInstanceID());
        }
        private static void ClearPool(int key)
        {
            if (!_particles.TryGetValue(key, out var pool))
            {
                return;
            }

            pool.Dispose();
            pool.Clear();
            _particles.Remove(key);
        }

    }
}