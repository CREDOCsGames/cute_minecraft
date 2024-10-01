using PlatformGame.Pipeline;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Character.Collision
{
    public delegate void HitEvent(HitBoxCollision collision);

    public struct HitBoxCollision
    {
        public Transform Victim;
        public Transform Attacker;
        public HitBoxCollider Subject;
    }

    public interface IHitBox
    {
        public Transform Actor { get; set; }
        public bool IsDelay { get; }
        public bool IsAttacker { get; set; }
        public void DoHit(HitBoxCollision collision);
    }

    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class HitBoxCollider : MonoBehaviour, IHitBox
    {
        public float HitDelay;
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
        Pipeline<HitBoxCollision> mHitPipeline;
        [SerializeField] UnityEvent<HitBoxCollision> mHitEvent;
        List<Transform> mHitBoxes = new();
        public void StartDelay()
        {
            mLastHitTime = Time.time;
        }

        public void SetAttacker(bool bAttacker)
        {
            IsAttacker = bAttacker;
        }

        public void AddHitEvent(UnityAction<HitBoxCollision> hitEvent)
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

        void SendCollisionData(IHitBox victim)
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

        public void DoHit(HitBoxCollision collision)
        {
            collision.Subject = this;
            mHitPipeline.Invoke(collision);
        }

        bool CanAttack(IHitBox targetHitBox)
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

            var victim = other.GetComponent<IHitBox>();
            if (victim == null)
            {
                return;
            }

            if (mLastHitTime < Time.time)
            {
                mHitBoxes.Clear();
            }

            if (mHitBoxes.Contains(victim.Actor))
            {
                return;
            }

            if (!CanAttack(victim))
            {
                return;
            }

            mHitBoxes.Add(victim.Actor);
            StartDelay();
            SendCollisionData(victim);
        }


        void Start()
        {
            Debug.Assert(Actor, $"Actor not found : {gameObject.name}");
            Debug.Assert(GetComponents<Collider>().Any(x => x.isTrigger), $"Trigger not found : {gameObject.name}");
        }

        protected virtual void Awake()
        {
<<<<<<< HEAD
            mLastHitTime = Time.time - HitDelay + 0.1f;
=======
>>>>>>> parent of e29ba99d (Merge pull request #148 from 1506022022/main)
            mHitPipeline = Pipelines.Instance.HitBoxColliderPipeline;
            mHitPipeline.InsertPipe(InvokeHitEvent);
        }

    }
}