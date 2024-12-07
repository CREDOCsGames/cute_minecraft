using PlatformGame.Debugger;
using Puzzle;
using UnityEngine;
using Util;

namespace Util
{
    public static class ReaderFactory
    {
        public static DataReader GetReader<T>(T t) where T : MonoBehaviour
        {
            if (t is Flower)
            {
                return new FlowerReader();
            }
            return new FailReader();
        }
    }

    public abstract class DataReader
    {
        public abstract bool IsReadable(byte[] data);
    }

    public class FailReader : DataReader
    {
        public override bool IsReadable(byte[] data)
        {
            return false;
        }
    }
    public class FlowerReader : DataReader
    {
        public override bool IsReadable(byte[] data)
        {
#if UNITY_EDITOR
            if (data.Length != 4)
            {
                Debug.LogWarning($"Invalid messages : {DebugLog.GetStrings(data)}");
            }
#endif
            return data.Length == 4;
        }
    }
}

public class SystemReader : DataReader
{
    public override bool IsReadable(byte[] data)
    {
#if UNITY_EDITOR
        if (data.Length != 3)
        {
            Debug.LogWarning($"Invalid messages : {DebugLog.GetStrings(data)}");
        }
#endif
        return data.Length == 3;
    }
}
