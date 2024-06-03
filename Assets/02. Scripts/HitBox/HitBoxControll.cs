using PlatformGame.Character.Collision;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class HitBoxColliderPair
{
    public string Name;
    public HitBoxCollider Collider;
}

[Serializable]
public class HitBoxControll
{
    [SerializeField] List<HitBoxColliderPair> pairs;

    List<HitBoxColliderPair> Pairs
    {
        get
        {
#if UNITY_EDITOR
            Dictionary<HitBoxCollider, string> duplicates = new();
            foreach (var pair in pairs)
            {
                Debug.Assert(!duplicates.ContainsKey(pair.Collider), $"Duplicate value : {pair.Collider.name}");
                duplicates.Add(pair.Collider, pair.Name);
            }
#endif
            return pairs;
        }
    }

    public List<HitBoxCollider> Colliders => Pairs.Select(x => x.Collider).ToList();
    public float Delay;
    public bool UseSyncDelay;

    public void StartDelay()
    {
        if (!UseSyncDelay)
        {
            return;
        }

        foreach (var collider in Colliders)
        {
            collider.StartDelay();
        }
    }

    public HitBoxCollider GetColliderAs(string filter)
    {
        Debug.Assert(pairs.Any(x => x.Name.Equals(filter)), $"Values that are not registered in Pairs : {filter}");
        var colliders = Pairs;
        return colliders[colliders.FindIndex(x => x.Name.Equals(filter))].Collider;
    }
}