using PlatformGame.Character;
using PlatformGame.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static PlatformGame.Character.Character;

public class Jailers : MonoBehaviour
{
    public Func<Collider, bool> ExclusionCondition;
    public List<Collider>[] StopLayers { get; private set; }
    public UnityEvent<Collider> MissesEvent;
    public List<UnityEvent<Collider>> SpotsEvent;
    [SerializeField] List<TriggerEventHandler> mSides;

    void SpotsTarget(Collider prisoner, int priority)
    {
        if (ExclusionCondition(prisoner))
        {
            return;
        }

        if (!StopLayers[priority].Contains(prisoner))
        {
            StopLayers[priority].Add(prisoner);
        }
        SpotsEvent[priority].Invoke(prisoner);
    }

    void MissesTarget(Collider prisoner, int priority)
    {
        if (!StopLayers[priority].Contains(prisoner))
        {
            return;
        }

        StopLayers[priority].Remove(prisoner);

        if (StopLayers.All(x => x.Count == 0))
        {
            MissesEvent.Invoke(prisoner);
        }
    }

    void Awake()
    {
        ExclusionCondition = (c) => !c.GetComponent<Character>()?.CompareTag(TAG_PLAYER) ?? true;

        StopLayers = new List<Collider>[mSides.Count];
        for (int i = 0; i < mSides.Count; i++)
        {
            StopLayers[i] = new List<Collider>();
        }

        for (int i = 0; i < mSides.Count; i++)
        {
            int priority = i;
            mSides[i].mOnCollisionEnter.AddListener((c) => SpotsTarget(c, priority));
            mSides[i].mOnCollisionExit.AddListener((c) => MissesTarget(c, priority));
        }
    }

}
