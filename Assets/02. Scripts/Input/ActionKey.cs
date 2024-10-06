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

        static readonly Dictionary<string, bool> mKeyDownMap = new Dictionary<string, bool>
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

        static Dictionary<string, bool> KeyDownMap
        {
            get
            {
                Debug.Assert(mKeyDownMap != null);
                return mKeyDownMap;
            }
        }

        public static List<string> InputKeys => KeyDownMap.Keys.ToList();
        static float mLastUpdate;

        public static Dictionary<string, bool> GetAxisRawMap()
        {
            if (!(mLastUpdate < Time.time))
            {
                return mKeyDownMap;
            }

            foreach (var button in KeyDownMap.ToList())
            {
                mKeyDownMap[button.Key] = UnityEngine.Input.GetAxisRaw(button.Key) != 0;
            }

            mLastUpdate = Time.time;

            return mKeyDownMap;
        }
    }
}