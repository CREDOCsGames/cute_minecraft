using UnityEngine;

namespace PlatformGame.Util
{
    public class BoolComponent : Condition
    {
        [SerializeField] ConditionData mCondition;
        bool mbCondition;

        public override bool IsTrue()
        {
            return mbCondition;
        }

        public void SetCondition(ConditionData condition)
        {
            mCondition = condition;
            mbCondition = false;
        }

        public void MeetCondition()
        {
            if (!ContainCondition(mCondition))
            {
                return;
            }
            mbCondition = true;
        }

        public void CancleCondition()
        {
            if (!ContainCondition(mCondition))
            {
                return;
            }
            mbCondition = false;
        }

        public override void SetFalse()
        {
            CancleCondition();
        }

        public void ToggleCondition()
        {
            if (!ContainCondition(mCondition))
            {
                return;
            }
            mbCondition = !mbCondition;
        }

        static bool ContainCondition(ConditionData condition)
        {
#if DEVELOPMENT
            if (condition == null)
            {
                Debug.Log($"Condition is null {condition.name}");
            }
#endif
            return condition;
        }

    }
}