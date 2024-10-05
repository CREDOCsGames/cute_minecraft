using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Input
{
    public static class ActionKey
    {
        public enum Button
        {
            Up, Down, Right, Left, Jump, Tab, Attack, Guard, DebugLoad, Cancel
        }

        public const string KEY_UP = "Up";
        public const string KEY_DOWN = "Down";
        public const string KEY_RIGHT = "Right";
        public const string KEY_LEFT = "Left";
        public const string KEY_JUMP = "Jump";
        public const string KEY_SWAP = "Tab";
        public const string KEY_ATTACK = "Attack";
        public const string KEY_GUARD = "Guard";
        public const string KEY_Debug = "DebugLoad";
        public const string KEY_Cancel = "Cancel";

        static readonly Dictionary<string, bool> mKeyDownMap = new Dictionary<string, bool>
        {
            { KEY_UP, false },
            { KEY_DOWN, false },
            { KEY_RIGHT, false },
            { KEY_LEFT, false },
            { KEY_JUMP, false },
            { KEY_SWAP, false },
            { KEY_ATTACK, false },
            { KEY_GUARD, false },
            { KEY_Debug, false },
            { KEY_Cancel, false }
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

        static Func<bool> mCheckKey;

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