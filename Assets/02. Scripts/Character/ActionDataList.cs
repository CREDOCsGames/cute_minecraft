using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    [CreateAssetMenu(fileName = "ActionDataList", menuName = "Custom/ActionDataList")]
    public class ActionDataList : ScriptableObject
    {
        [SerializeField] private List<ActionData> _actions;

        public List<ActionData> Actions
        {
            get
            {
                Debug.Assert(_actions is { Count: > 0 });
                return _actions;
            }
        }

        public Dictionary<int, ActionData> Library
        {
            get
            {
                var library = new Dictionary<int, ActionData>();
                foreach (var item in _actions)
                {
                    Debug.Assert(!library.ContainsKey(item.ID), $"Duplicate values : {item.ID}");
                    library.Add(item.ID, item);
                }

                return library;
            }
        }
    }
}