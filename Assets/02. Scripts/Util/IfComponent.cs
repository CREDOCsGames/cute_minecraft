using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class IFComponent : MonoBehaviour
    {
        [SerializeField] private List<Condition> _conditions;
        [SerializeField] private UnityEvent _trueEvent;

        public void Invoke()
        {
            InvokeConditions();
        }

        protected virtual bool InvokeConditions()
        {
            if (!IsTrue())
            {
                return false;
            }

            OnTrueEvent();
            return true;
        }

        public void AddListenerTrueEvent(UnityAction listener)
        {
            _trueEvent.AddListener(listener);
        }

        public void RemoveListenerTrueEvent(UnityAction listener)
        {
            _trueEvent.RemoveListener(listener);
        }

        public void AddCondition(Condition condition)
        {
            if (_conditions.Contains(condition))
            {
                Debug.Log($"already exist : {condition}");
                return;
            }

            _conditions.Add(condition);
        }

        public void RemoveCondition(Condition condition)
        {
            if (!_conditions.Contains(condition))
            {
                Debug.Log($"Not found : {condition}");
                return;
            }

            _conditions.Remove(condition);
        }

        public void SetFalseAllCondition()
        {
            foreach (var condition in _conditions)
            {
                condition.SetFalse();
            }
        }

        private bool IsTrue()
        {
            return _conditions.Count == 0 || _conditions.All(x => x.IsTrue());
        }

        private void OnTrueEvent()
        {
            _trueEvent.Invoke();
        }
    }
}