using UnityEngine;
using PlatformGame.Character.Combat;
using Unity.VisualScripting;

namespace PlatformGame.Character.BehaviorTree
{
    public class FocusOnWhoHitMe : Focusing
    {
        public float mMemoryTime = 5f;
        float mLastHitTime;
        Character mLastHitMe;

        public override void OnAwake()
        {
            base.OnAwake();
            AbilityLog.AddHitCallback(mMe.GetInstanceID(), LogHitMe);
        }

        protected override Transform GetTargetInFocus()
        {
            if(mLastHitTime + mMemoryTime < Time.time)
            {
                mLastHitMe = null;
            }
            return mLastHitMe?.transform;
        }

        void LogHitMe(AbilityCollision collision)
        {
            if(Owner.IsDestroyed())
            {
                AbilityLog.RemoveHitCallback(mMe.GetInstanceID(), LogHitMe);
                return;
            }
            mLastHitMe = collision.Caster;
            mLastHitTime = Time.time;
        }


    }

}