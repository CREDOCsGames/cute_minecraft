using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public class OrderConditionComponent : Condition
    {
        [SerializeField] private List<ConditionData> _conditions;
        private readonly List<ConditionData> _enters = new();

        public override bool IsTrue()
        {
            if (_conditions.Count == 0)
            {
                Debug.Log(_enters.Count);
                return false;
            }

            if (_conditions.Count != _enters.Count)
            {
                Debug.Log($"{_conditions.Count} : {_enters.Count}");
                return false;
            }

            for (var i = 0; i < _conditions.Count; i++)
            {
                Debug.Log($"{_conditions[i].name} : {_enters[i].name}");
                if (_conditions[i] == _enters[i])
                {
                    continue;
                }

                return false;
            }

            return true;
        }

        public override void SetFalse()
        {
            ClearEnter();
        }

        public void ClearEnter()
        {
            _enters.Clear();
        }

        public void EnterBool(ConditionData condition)
        {
            _enters.Add(condition);
        }
    }
}