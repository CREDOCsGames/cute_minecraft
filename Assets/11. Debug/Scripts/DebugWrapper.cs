using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Debugger
{
    public static class DebugWrapper
    {
        public static readonly Dictionary<int, Dictionary<string, string>> FrameTagMassages = new();

        public static void LogMessage(int instanceID, string message, bool printLog = false)
        {
            if (printLog)
            {
                Debug.Log(message);
            }

            int rangeOfID = DebugLog.ID_ABILITY.Length;
            var ID = message.Substring(0, rangeOfID);
            var body = message.Substring(rangeOfID, message.Length - rangeOfID);

            GetOrAddValue(FrameTagMassages, instanceID, new Dictionary<string, string>());
            GetOrAddValue(FrameTagMassages[instanceID], ID, null);
            FrameTagMassages[instanceID][ID] = body;
        }

        public static void Assert(bool condition)
        {
            Debug.Assert(condition);
        }

        public static void Assert(bool condition, object message)
        {
            Debug.Assert(condition, message);
        }

        public static Dictionary<K, V> GetOrAddValue<K, V>(Dictionary<K, V> dictionary, K key, V value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
            return dictionary;
        }

    }
}