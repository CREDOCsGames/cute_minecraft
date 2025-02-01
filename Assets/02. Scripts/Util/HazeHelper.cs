using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class HazeHelper : MonoBehaviour
{
    [SerializeField] private ParticleSystem _collisionEffect;
    [SerializeField] private ParticleSystem _origin;
    public ObjectPool<ParticleSystem> Pool
    {
        get
        {
            if (_pool == null)
            {
                _pool = new ObjectPool<ParticleSystem>(() => GameObject.Instantiate(_origin));
            }
            return _pool;
        }
    }
    private ObjectPool<ParticleSystem> _pool;
    private List<Transform> _spawns = new();
    public void PlayEffect(Collider coll)
    {
        var newPosition = coll.transform.position;
        if (_spawns.Contains(coll.transform.root))
        {
            return;
        }
        _spawns.Add(coll.transform.root);
        var effect = Pool.Get();
        effect.transform.position = newPosition;
        effect.Play();
        StartCoroutine(ReleaseEffect(effect,coll.transform.root));
    }

    private IEnumerator ReleaseEffect(ParticleSystem particle, Transform root)
    {
        yield return new WaitForSeconds(particle.totalTime);
        Pool.Release(particle);
        _spawns.Remove(root);
    }
}
