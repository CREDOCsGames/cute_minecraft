using System.Collections.Generic;
using UnityEngine;

namespace Util
{

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
    public class FlowerReader : NW.DataReader
    {
        public override bool IsReadable(byte[] data)
        {
#if UNITY_EDITOR
            if (data.Length != 4)
            {
                Debug.LogWarning($"Invalid messages : {PlatformGame.Debugger.DebugLog.GetStrings(data)}");
            }
#endif
            return data.Length == 4;
        }
    }
}

public class SystemReader : NW.DataReader
{
    public static readonly byte[] FAIL = { 0 };
    public static readonly byte[] CLEAR_TOP_FACE = { 1 };
    public static readonly byte[] CLEAR_LEFT_FACE = { 2 };
    public static readonly byte[] CLEAR_FRONT_FACE = { 3 };
    public static readonly byte[] CLEAR_RIGHT_FACE = { 4 };
    public static readonly byte[] CLEAR_BACK_FACE = { 5 };
    public static readonly byte[] CLEAR_BOTTOM_FACE = { 6 };
    public static readonly List<byte[]> CLEAR_MESSAGES = new() { CLEAR_TOP_FACE, CLEAR_LEFT_FACE, CLEAR_FRONT_FACE, CLEAR_RIGHT_FACE, CLEAR_BACK_FACE, CLEAR_BOTTOM_FACE };
    public static bool IsClearFace(byte[] data)
    {
        return data.Equals(CLEAR_TOP_FACE)
            || data.Equals(CLEAR_LEFT_FACE)
            || data.Equals(CLEAR_FRONT_FACE)
            || data.Equals(CLEAR_RIGHT_FACE)
            || data.Equals(CLEAR_BACK_FACE)
            || data.Equals(CLEAR_BOTTOM_FACE);
    }

    public override bool IsReadable(byte[] data)
    {
        return data.Length == 1;
    }
}
