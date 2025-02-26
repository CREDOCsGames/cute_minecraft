using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Util
{
    public static class ParticlePlayer
    {
        public static void PlayParticleAt(ParticleSystem particle, Vector3 position)
        {
            var instance = GameObjectPool<ParticleSystem>.GetPool(particle).Get();
            instance.transform.position = position;
            instance.Play();
            CoroutineRunner.instance.StartCoroutine(ReleaseEffect(particle, instance));
        }
        public static void PlayParticleAt(ParticleSystem particle, Vector3 position, Vector3 scale)
        {
            var instance = GameObjectPool<ParticleSystem>.GetPool(particle).Get();
            instance.transform.position = position;
            instance.transform.localScale = scale;
            instance.Play();
            CoroutineRunner.instance.StartCoroutine(ReleaseEffect(particle, instance));
        }
        private static IEnumerator ReleaseEffect(ParticleSystem origin, ParticleSystem particle)
        {
            yield return new WaitUntil(() => !particle.IsAlive());
            GameObjectPool<ParticleSystem>.GetPool(origin).Release(particle);
        }
    }

}
