using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public struct HitBoxCollision
    {
        public Transform Victim;
        public Transform Attacker;
    }

    public interface IHitBox
    {
        public HitBox HitBox { get; }
    }

    [Serializable]
    public class HitBox
    {
        [SerializeField] float mHitDelay;
        public float HitDelay
        {
            get => mHitDelay;
            set => mHitDelay = value;
        }
        [SerializeField] bool mbAttacker;
        public bool IsAttacker
        {
            get => mbAttacker;
            set => mbAttacker = value;
        }

        [SerializeField] Transform mActor;
        public Transform Actor
        {
            get => mActor;
            set => mActor = value;
        }
        public bool IsDelay => mLastHitTime < Time.time && Time.time < mLastHitTime + HitDelay;
        float mLastHitTime;
        readonly List<Transform> mHits = new();
        [SerializeField] UnityEvent<HitBoxCollision> mHitEvent;

        public void AddHitEvent(UnityAction<HitBoxCollision> hitEvent)
        {
            mHitEvent.AddListener(hitEvent);
        }

        public void RemoveHitEvent(UnityAction<HitBoxCollision> hitEvent)
        {
            mHitEvent.RemoveListener(hitEvent);
        }

        public void CheckHit(Collider other)
        {
            if (!IsAttacker)
            {
                return;
            }

            if (!other.TryGetComponent<IHitBox>(out var victim))
            {
                return;
            }

            if (mLastHitTime < Time.time)
            {
                mHits.Clear();
            }

            if (mHits.Contains(victim.HitBox.Actor))
            {
                return;
            }

            if (!CanAttack(victim.HitBox))
            {
                return;
            }

            mHits.Add(victim.HitBox.Actor);
            SendCollisionData(victim.HitBox);
        }

        void StartDelay()
        {
            mLastHitTime = Time.time;
        }

        void InvokeHitEvent(HitBoxCollision collision)
        {
            mHitEvent.Invoke(collision);
        }

        void SendCollisionData(HitBox victim)
        {
            var attacker = this;
            var collsion = new HitBoxCollision()
            {
                Attacker = attacker.Actor,
                Victim = victim.Actor,
            };
            victim.DoHit(collsion);
            attacker.DoHit(collsion);
        }

        void DoHit(HitBoxCollision collision)
        {
            InvokeHitEvent(collision);
            StartDelay();
        }

        bool CanAttack(HitBox targetHitBox)
        {
            return IsAttacker &&
                   !IsDelay &&
                   !targetHitBox.IsAttacker &&
                   !Actor.Equals(targetHitBox.Actor);
        }

    }
}
