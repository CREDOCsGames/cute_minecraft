using System;
using UnityEngine;

namespace Battle
{
    public class CollisionBox
    {
        public Transform Actor
        {
            get; private set;
        }
        public event Action<HitBoxCollision> OnCollision;

        public CollisionBox(Transform actor)
        {
            Actor = actor;
        }
        protected static void InvokeCollision(CollisionBox attack, CollisionBox hit)
        {
            var collsion = new HitBoxCollision()
            {
                Attacker = attack.Actor,
                Victim = hit.Actor,
            };
            attack.InvokeEvent(collsion);
            hit.InvokeEvent(collsion);
        }
        private void InvokeEvent(HitBoxCollision collision)
        {
            OnCollision.Invoke(collision);
        }
    }

}
