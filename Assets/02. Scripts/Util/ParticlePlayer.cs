using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace Util
{
    public static class ParticlePlayer
    {
        private static readonly Dictionary<ParticleSystem, ObjectPool<ParticleSystem>> _particles = new();

        public static ObjectPool<ParticleSystem> GetPool(ParticleSystem origin)
        {
            _particles.TryAdd(origin, new(() => GameObject.Instantiate(origin)));
            return _particles[origin];
        }
        public static void ClearAll()
        {
            _particles.Keys
                      .ToList()
                      .ForEach(ClearPool);
        }
        public static void ClearPool(ParticleSystem origin)
        {
            GetPool(origin).Dispose();
            GetPool(origin).Clear();
            _particles.Remove(origin);
        }

    }
}