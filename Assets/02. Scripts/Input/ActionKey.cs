using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Input1
{
    public static class ActionKey
    {
        public enum Button
        {
            Up,
            Down,
            Right,
            Left,
            Jump,
            Tab,
            Attack,
            Guard,
            DebugLoad,
            Cancel
        }

        private static readonly Dictionary<string, bool> _keyDownMap = new()
        {
            { Button.Up.ToString(), false },
            { Button.Down.ToString(), false },
            { Button.Right.ToString(), false },
            { Button.Left.ToString(), false },
            { Button.Jump.ToString(), false },
            { Button.Attack.ToString(), false },
            { Button.Guard.ToString(), false },
            { Button.DebugLoad.ToString(), false },
            { Button.Cancel.ToString(), false }
        };

        private static Dictionary<string, bool> KeyDownMap
        {
            get
            {
                Debug.Assert(_keyDownMap != null);
                return _keyDownMap;
            }
        }

        public static List<string> InputKeys => KeyDownMap.Keys.ToList();
        private static float _lastUpdate;

        public static Dictionary<string, bool> GetAxisRawMap()
        {
            if (!(_lastUpdate < Time.time))
            {
                return _keyDownMap;
            }

            foreach (var button in KeyDownMap.ToList())
            {
                _keyDownMap[button.Key] = UnityEngine.Input.GetAxisRaw(button.Key) != 0;
            }

            _lastUpdate = Time.time;

            return _keyDownMap;
        }
    }
}