using UnityEngine;

namespace Util
{
    public class BoolComponent : Condition
    {
        [SerializeField] private ConditionData _condition;
        private bool _bCondition;

        public override bool IsTrue()
        {
            return _bCondition;
        }

        public void SetCondition(ConditionData condition)
        {
            _condition = condition;
            _bCondition = false;
        }

        public void MeetCondition()
        {
            if (!ContainCondition(_condition))
            {
                return;
            }

            _bCondition = true;
        }

        public void CancleCondition()
        {
            if (!ContainCondition(_condition))
            {
                return;
            }

            _bCondition = false;
        }

        public override void SetFalse()
        {
            CancleCondition();
        }

        public void ToggleCondition()
        {
            if (!ContainCondition(_condition))
            {
                return;
            }

            _bCondition = !_bCondition;
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