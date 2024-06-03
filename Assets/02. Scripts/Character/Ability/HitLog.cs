using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Character.Collision
{
    [CreateAssetMenu(menuName = "Custom/Pipeline/HitLog")]
    public class HitLog : ScriptableObject
    {
        static readonly Dictionary<int, UnityEvent<HitBoxCollision>> mHitCallback = new();

        public static void AddHitCallback(int instanceID, UnityAction<HitBoxCollision> call)
        {
            if (!mHitCallback.ContainsKey(instanceID))
            {
                mHitCallback.Add(instanceID, new UnityEvent<HitBoxCollision>());
            }

            mHitCallback[instanceID].AddListener(call);
        }

        public void OnHit(HitBoxCollision collision)
        {
            var victim = collision.Victim.GetInstanceID();
            if (mHitCallback.TryGetValue(victim, out var victimCallback))
            {
                victimCallback.Invoke(collision);
            }

            var attacker = collision.Attacker.GetInstanceID();
            if (mHitCallback.TryGetValue(attacker, out var attackerCallback))
            {
                attackerCallback.Invoke(collision);
            }
        }
    }
}