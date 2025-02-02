using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public class ParticlePlayerComponent : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _originEffect;
        private readonly List<Transform> _spawns = new();

        public void PlayEffectThisTransform(Transform spawnPoint)
        {
            PlayEffect(spawnPoint, spawnPoint);
        }
        public void PlayEffectThisCollider(Collider coll)
        {
            PlayEffect(coll.transform, coll.transform.root);
        }
        private void PlayEffect(Transform spawnPoint, Transform root)
        {
            var newPosition = spawnPoint.position;
            if (_spawns.Contains(root))
            {
                return;
            }
            _spawns.Add(spawnPoint);
            var effect = ParticlePlayer.GetPool(_originEffect).Get();
            Debug.Log($"Step : {effect.GetInstanceID()}");
            effect.transform.position = newPosition;
            effect.Play();
            StartCoroutine(ReleaseEffect(effect, root));
        }
        private IEnumerator ReleaseEffect(ParticleSystem particle, Transform root)
        {
            yield return new WaitForSeconds(particle.main.duration);
            ParticlePlayer.GetPool(_originEffect).Release(particle);
            Debug.Log($"End : {particle.GetInstanceID()}");
            _spawns.Remove(root);
        }

    }
}
