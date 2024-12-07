using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class CommandListComponent : MonoBehaviour
    {
        private int _index;
        [Header("References")]
        [SerializeField] private List<UnityEvent> Commands;
        [Header("Options")]
        [SerializeField] private bool _useClamp;

        public void InvokeCommand()
        {
            if (_useClamp && Commands.Count <= _index)
            {
                _index = 0;
            }

            if (Commands.Count <= _index)
            {
                Debug.Log($"Index out of range : {_index}/{Commands.Count} {name}");
                return;
            }

            Commands[_index].Invoke();
            _index++;
        }

        public void Clear()
        {
            Commands.Clear();
        }

        public void Init()
        {
            _index = 0;
        }
    }
}