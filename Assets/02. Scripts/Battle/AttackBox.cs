using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Battle
{
    public class AttackBox : CollisionBox
    {
        private readonly Delay _delay;
        private readonly List<Collider> _attacked = new();
        private bool _NotWithinAttackWindow => !_delay.IsDelay();
        public AttackBox(Transform actor, float attackWindow = 0f) : base(actor)
        {
            _delay = new Delay(attackWindow);
            _delay.StartTime = -1f;
        }

        public void CheckCollision(Collider other)
        {
            if (_NotWithinAttackWindow ||
                !other.TryGetComponent<IHitBox>(out var victim) ||
                victim.HitBox.Actor.Equals(Actor) ||
                _attacked.Contains(other))
            {
                return;
            }

            _attacked.Add(other);
            CollisionBox.InvokeCollision(this, victim.HitBox);
        }

        public void OpenAttackWindow()
        {
            _delay.SetStartTime();
            _attacked.Clear();
        }

    }
}