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
        public void PlayEffectThisCollider(Collision coll)
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
            effect.transform.position = newPosition;
            effect.Play();
            var coll = spawnPoint.GetComponent<BoxCollider>();
            effect.transform.localScale = coll ? Vector3.Scale(coll.size, spawnPoint.transform.localScale) : Vector3.one;
            StartCoroutine(ReleaseEffect(effect, root));
        }
        private IEnumerator ReleaseEffect(ParticleSystem particle, Transform root)
        {
            yield return new WaitForSeconds(particle.main.duration);
            ParticlePlayer.GetPool(_originEffect).Release(particle);
            _spawns.Remove(root);
        }

    }
}
