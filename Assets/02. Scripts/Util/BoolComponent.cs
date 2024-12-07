using UnityEngine;

namespace Util
{
    public class BoolComponent : Condition
    {
        [SerializeField] private ConditionData mCondition;
        private bool _condition;

        public override bool IsTrue()
        {
            return _condition;
        }

        public void SetCondition(ConditionData condition)
        {
            mCondition = condition;
            _condition = false;
        }

        public void MeetCondition()
        {
            if (!ContainCondition(mCondition))
            {
                return;
            }

            _condition = true;
        }

        public void CancleCondition()
        {
            if (!ContainCondition(mCondition))
            {
                return;
            }

            _condition = false;
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

            _condition = !_condition;
        }

        private static bool ContainCondition(ConditionData condition)
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