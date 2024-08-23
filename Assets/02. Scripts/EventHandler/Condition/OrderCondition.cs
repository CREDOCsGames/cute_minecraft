using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Util
{
    public class OrderCondition : Condition
    {
        [SerializeField] List<ConditionData> mConditions;
        List<ConditionData> mEnters = new();

        public override bool IsTrue()
        {
            if (mConditions.Count == 0)
            {
                Debug.Log(mEnters.Count);
                return false;
            }

            if (mConditions.Count != mEnters.Count)
            {
                Debug.Log($"{mConditions.Count} : {mEnters.Count}");
                return false;
            }

            for (int i = 0; i < mConditions.Count; i++)
            {
                Debug.Log($"{mConditions[i].name} : {mEnters[i].name}");
                if (mConditions[i] == mEnters[i])
                {
                    continue;
                }
                return false;
            }

            return true;
        }

        public override void SetFalseAll()
        {
            ClearEnter();
        }

        public void ClearEnter()
        {
            mEnters.Clear();
        }

        public void EnterBool(ConditionData condition)
        {
            mEnters.Add(condition);
        }
    }

}
