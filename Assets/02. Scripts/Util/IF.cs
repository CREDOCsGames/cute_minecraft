using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class IF : MonoBehaviour
    {
        [SerializeField] List<Condition> mConditions;
        [SerializeField] UnityEvent TrueEvent;

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
            TrueEvent.AddListener(listener);
        }

        public void RemoveListenerTrueEvent(UnityAction listener)
        {
            TrueEvent.RemoveListener(listener);
        }

        public void AddCondition(Condition condition)
        {
            if (mConditions.Contains(condition))
            {
                Debug.Log($"already exist : {condition}");
                return;
            }

            mConditions.Add(condition);
        }

        public void RemoveCondition(Condition condition)
        {
            if (!mConditions.Contains(condition))
            {
                Debug.Log($"Not found : {condition}");
                return;
            }

            mConditions.Remove(condition);
        }

        public void SetFalseAllCondition()
        {
            foreach (var condition in mConditions)
            {
                condition.SetFalse();
            }
        }

        bool IsTrue()
        {
            return mConditions.Count == 0 || mConditions.All(x => x.IsTrue());
        }

        void OnTrueEvent()
        {
            TrueEvent.Invoke();
        }
    }
}