using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Battle
{
    public class AttackBox : CollisionBox
    {
        public enum AttackType { None, OneHit };
        private readonly Delay _delay;
        private readonly List<Collider> _attacked = new();
        private bool _bNotWithinAttackWindow => !_delay.IsDelay() || (_attackType == AttackType.OneHit && _bHit);
        private bool _bHit;
        private AttackType _attackType;

        public AttackBox(Transform actor, float attackWindow = 0f) : base(actor)
        {
            _delay = new Delay(attackWindow);
            _delay.StartTime = -1f;
        }

        public void CheckCollision(Collider other)
        {
            if (_bNotWithinAttackWindow)
            {
                return;
            }

            if (_bNotWithinAttackWindow ||
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
            _delay.DoStart();
            _attacked.Clear();
        }

        public void SetType(AttackType type)
        {
            _attackType = type;
        }

    }
}