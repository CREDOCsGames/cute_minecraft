using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Util
{
    public class ConditionEventHandler : MonoBehaviour
    {
        Dictionary<ConditionData, bool> mConditionStatusMap = new();
        public UnityEvent CompletionEvent;

        public void AddCondition(ConditionData condition)
        {
            if (mConditionStatusMap.ContainsKey(condition))
            {
                Debug.Log($"Conditions already added : {condition}");
                return;
            }
            mConditionStatusMap.Add(condition, false);
        }

        public void RemoveCondition(ConditionData condition)
        {
            if (!mConditionStatusMap.ContainsKey(condition))
            {
                Debug.Log($"Conditions that don't exist : {condition}");
                return;
            }
            mConditionStatusMap.Remove(condition);
        }

        public void MeetCondition(ConditionData condition)
        {
            if (!mConditionStatusMap.ContainsKey(condition))
            {
                Debug.Log($"Conditions that don't exist : {condition}");
                return;
            }
            mConditionStatusMap[condition] = true;

            if (mConditionStatusMap.All(x => x.Value))
            {
                CompletionEvent.Invoke();
            }
        }

        public void CancleCondition(ConditionData condition)
        {
            if (!mConditionStatusMap.ContainsKey(condition))
            {
                Debug.Log($"Conditions that don't exist : {condition}");
                return;
            }
            mConditionStatusMap[condition] = false;
        }

        public void ResetAllConditions()
        {
            foreach (var condition in mConditionStatusMap.Keys)
            {
                mConditionStatusMap[condition] = false;
            }
        }

    }

}
