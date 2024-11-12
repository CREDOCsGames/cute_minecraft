using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Battle
{
    public class AttackBox : CollisionBox
    {
        readonly Delay mDelay;
        readonly List<Collider> mAttacked = new List<Collider>();
        bool mbNotWithinAttackWindow => !mDelay.IsDelay();
        public AttackBox(Transform actor, float attackWindow = 0f) : base(actor)
        {
            mDelay = new Delay(attackWindow);
            mDelay.StartTime = -1f;
        }

        public void CheckCollision(Collider other)
        {
            if (mbNotWithinAttackWindow ||
                !other.TryGetComponent<IHitBox>(out var victim) ||
                victim.HitBox.Actor.Equals(Actor) ||
                mAttacked.Contains(other))
            {
                return;
            }

            mAttacked.Add(other);
            CollisionBox.InvokeCollision(this, victim.HitBox);
        }

        public void OpenAttackWindow()
        {
            mDelay.SetStartTime();
            mAttacked.Clear();
        }

    }
}