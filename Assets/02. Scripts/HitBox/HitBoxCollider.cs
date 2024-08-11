using PlatformGame.Pipeline;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Character.Collision
{
    public delegate void HitEvent(HitBoxCollision collision);

    public struct HitBoxCollision
    {
        public Character Victim;
        public Character Attacker;
        public HitBoxCollider Subject;
    }

    public interface IHitBox
    {
        public Character Actor { get; set; }
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

        [SerializeField] Character mActor;
        public Character Actor
        {
            get => mActor;
            set => mActor = value;
        }
        public bool IsDelay => Time.time < mLastHitTime + HitDelay;
        float mLastHitTime;
        Pipeline<HitBoxCollision> mHitPipeline;
        [SerializeField] UnityEvent<HitBoxCollision> mHitEvent;

        void StartDelay()
        {
            mLastHitTime = Time.time;
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
            StartDelay();
            collision.Subject = this;
            mHitPipeline.Invoke(collision);
        }

        bool CanAttack(IHitBox targetHitBox)
        {
            return IsAttacker &&
                   !targetHitBox.IsDelay &&
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

            if (!CanAttack(victim))
            {
                return;
            }

            SendCollisionData(victim);
        }


        void Start()
        {
            Debug.Assert(Actor, $"Actor not found : {gameObject.name}");
            Debug.Assert(GetComponents<Collider>().Any(x => x.isTrigger), $"Trigger not found : {gameObject.name}");
            Debug.Assert(GetComponent<Rigidbody>().isKinematic, $"Not set Kinematic : {gameObject.name}");
        }

        void Awake()
        {
            mLastHitTime = Time.time - HitDelay + 0.1f;
            mHitPipeline = Pipelines.Instance.HitBoxColliderPipeline;
            mHitPipeline.InsertPipe(InvokeHitEvent);
        }

    }
}