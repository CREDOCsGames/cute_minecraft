using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class ConditionEventHandler : MonoBehaviour
    {
        private readonly Dictionary<ConditionData, bool> _conditionStatusMap = new();
        public UnityEvent CompletionEvent;
        public UnityEvent ResetAllEvetn;

        public void AddCondition(ConditionData condition)
        {
            if (_conditionStatusMap.TryAdd(condition, false))
            {
                return;
            }

            Debug.Log($"Conditions already added : {condition}");
            return;
        }

        public void RemoveCondition(ConditionData condition)
        {
            if (!_conditionStatusMap.ContainsKey(condition))
            {
                Debug.Log($"Conditions that don't exist : {condition}");
                return;
            }

            _conditionStatusMap.Remove(condition);
        }

        public void MeetCondition(ConditionData condition)
        {
            if (!_conditionStatusMap.ContainsKey(condition))
            {
                Debug.Log($"Conditions that don't exist : {condition}");
                return;
            }

            _conditionStatusMap[condition] = true;

            if (_conditionStatusMap.All(x => x.Value))
            {
                CompletionEvent.Invoke();
            }
        }

        public void CancleCondition(ConditionData condition)
        {
            if (!_conditionStatusMap.ContainsKey(condition))
            {
                Debug.Log($"Conditions that don't exist : {condition}");
                return;
            }

            _conditionStatusMap[condition] = false;

            if (_conditionStatusMap.All(x => !x.Value))
            {
                ResetAllEvetn.Invoke();
            }
        }

        public void ToggleCondition(ConditionData condition)
        {
            if (!_conditionStatusMap.ContainsKey(condition))
            {
                Debug.Log($"Conditions that don't exist : {condition}");
                return;
            }

            if (_conditionStatusMap[condition])
            {
                CancleCondition(condition);
            }
            else
            {
                MeetCondition(condition);
            }
        }

        public void ResetAllConditions()
        {
            var keys = _conditionStatusMap.Keys.ToList();
            foreach (var key in keys)
            {
                _conditionStatusMap[key] = false;
            }

            ResetAllEvetn.Invoke();
        }
    }
}