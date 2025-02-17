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
            var position = spawnPoint.position;
            if (_spawns.Contains(root))
            {
                return;
            }
            _spawns.Add(spawnPoint);

            var coll = spawnPoint.GetComponent<BoxCollider>();
            var size = coll ? Vector3.Scale(coll.size, spawnPoint.transform.localScale) : Vector3.one;
            ParticlePlayer.PlayParticleAt(_originEffect, position, size);
        }

    }
}
