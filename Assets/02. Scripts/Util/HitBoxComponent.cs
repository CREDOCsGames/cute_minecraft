using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public struct HitBoxCollision
    {
        public Transform Victim;
        public Transform Attacker;
    }

    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class HitBoxComponent : MonoBehaviour
    {
        public float HitDelay;
        [SerializeField] bool mbAttacker;

        protected bool IsAttacker
        {
            get => mbAttacker;
            set => mbAttacker = value;
        }

        [SerializeField] Transform mActor;
        protected Transform Actor => mActor;

        protected bool IsDelay => mLastHitTime < Time.time && Time.time < mLastHitTime + HitDelay;
        float mLastHitTime;
        [SerializeField] UnityEvent<HitBoxCollision> mHitEvent;
        readonly List<Transform> mHits = new();

        void StartDelay()
        {
            mLastHitTime = Time.time;
        }

        public void SetAttacker(bool bAttacker)
        {
            IsAttacker = bAttacker;
        }

        protected void AddHitEvent(UnityAction<HitBoxCollision> hitEvent)
        {
            mHitEvent.AddListener(hitEvent);
        }

        public void RemoveHitEvent(UnityAction<HitBoxCollision> hitEvent)
        {
            mHitEvent.RemoveListener(hitEvent);
        }

        void InvokeHitEvent(HitBoxCollision collision)
        {
            mHitEvent.Invoke(collision);
        }

        void SendCollisionData(HitBoxComponent victim)
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
        }

        bool CanAttack(HitBoxComponent targetHitBox)
        {
            return IsAttacker &&
                   !IsDelay &&
                   !targetHitBox.IsAttacker &&
                   !Actor.Equals(targetHitBox.Actor);
        }

        void OnTriggerStay(Collider other)
        {
            if (!IsAttacker)
            {
                return;
            }

            if (!other.TryGetComponent<HitBoxComponent>(out var victim))
            {
                return;
            }

            if (mLastHitTime < Time.time)
            {
                mHits.Clear();
            }

            if (mHits.Contains(victim.Actor))
            {
                return;
            }

            if (!CanAttack(victim))
            {
                return;
            }

            mHits.Add(victim.Actor);
            StartDelay();
            SendCollisionData(victim);
        }

        void Start()
        {
            Debug.Assert(Actor, $"Actor not found : {gameObject.name}");
            Debug.Assert(GetComponents<Collider>().Any(x => x.isTrigger), $"Trigger not found : {gameObject.name}");
        }
    }
}