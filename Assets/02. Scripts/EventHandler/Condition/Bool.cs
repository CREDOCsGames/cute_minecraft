using UnityEngine;

namespace PlatformGame.Util
{
    public class Bool : Condition
    {
        [SerializeField] ConditionData mCondition;
        (ConditionData, bool) mConditionState = new();

        public override bool IsTrue()
        {
            return mConditionState.Item2;
        }

        public void SetCondition(ConditionData condition)
        {
            mConditionState.Item1 = condition;
            mConditionState.Item2 = false;
        }

        public void MeetCondition(ConditionData condition)
        {
            if (!ContainCondition(condition))
            {
                return;
            }
            mConditionState.Item2 = true;
        }

        public void CancleCondition(ConditionData condition)
        {
            if (!ContainCondition(condition))
            {
                return;
            }
            mConditionState.Item2 = false;
        }

        public override void SetFalseAll()
        {
            CancleCondition(mCondition);
        }

        public void ToggleCondition(ConditionData condition)
        {
            if (!ContainCondition(condition))
            {
                return;
            }
            mConditionState.Item2 = !mConditionState.Item2;
        }

        bool ContainCondition(ConditionData condition)
        {
            if (!mConditionState.Item1)
            {
                return false;
            }

            if (mConditionState.Item1 != condition)
            {
                return false;
            }
            return true;
        }

        void Awake()
        {
            if (mCondition == null)
            {
                return;
            }
            mConditionState.Item1 = mCondition;
        }

    }
}