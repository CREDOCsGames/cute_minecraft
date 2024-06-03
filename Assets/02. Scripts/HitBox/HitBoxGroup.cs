using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Character.Collision
{
    public delegate void AttackCallback(Character victim, uint actionID);

    public class HitBoxGroup : MonoBehaviour
    {
        Character mCharacter;
        [SerializeField] HitBoxControll mHitControll;
        [SerializeField] HitBoxControll mAttackControll;

        public void SetAttackEvent(List<string> filterColliderNames, HitEvent hitEvent, uint actionID)
        {
            var colliders = GetCollidersAs(filterColliderNames, mAttackControll);
            foreach (var hitBoxCollider in colliders)
            {
                hitBoxCollider.SetAbilityEvent(hitEvent);
            }
        }

        void OnHit(HitBoxCollision collision)
        {
            if (collision.Subject.IsAttacker)
            {
                mAttackControll.StartDelay();
            }
            else
            {
                mHitControll.StartDelay();
            }
        }

        void InitColliders(HitBoxControll hitBoxControll, bool isAttacker)
        {
            foreach (var collider in hitBoxControll.Colliders)
            {
                collider.Actor = mCharacter;
                collider.IsAttacker = isAttacker;
                collider.HitCallback.AddListener(OnHit);
            }
        }

        static List<HitBoxCollider> GetCollidersAs(List<string> filterColliderNames, HitBoxControll hitBoxControll)
        {
            var list = new List<HitBoxCollider>();
            foreach (var filter in filterColliderNames)
            {
                var collider = hitBoxControll.GetColliderAs(filter);
                list.Add(collider);
            }

            return list;
        }

        void Awake()
        {
            mCharacter = GetComponent<Character>();
            InitColliders(mHitControll, false);
            InitColliders(mAttackControll, true);
        }
    }
}