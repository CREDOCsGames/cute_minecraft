using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Battle
{
    public class AttackBox : CollisionBox
    {
        public enum Type { None, One };
        private readonly Delay _delay;
        private readonly List<Collider> _attacked = new();
        private bool _NotWithinAttackWindow => !_delay.IsDelay() || (_type == Type.One && _bHit);
        private bool _bHit;
        private Type _type;

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
            _bHit = true;
        }

        public void OpenAttackWindow()
        {
            _bHit = false;
            _delay.SetStartTime();
            _attacked.Clear();
        }

        public void SetType(Type type)
        {
            _type = type;
        }

    }
}