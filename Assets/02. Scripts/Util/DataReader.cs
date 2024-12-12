using PlatformGame.Debugger;
using Puzzle;
using UnityEngine;
using Util;

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
        //        public override bool IsReadable(byte[] data)
        //        {
        //#if UNITY_EDITOR
        //            if (data.Length != 4)
        //            {
        //                Debug.LogWarning($"Invalid messages : {DebugLog.GetStrings(data)}");
        //            }
        //#endif
        //            return data.Length == 4;
        //        }
    }
}

public class SystemReader : NW.DataReader
{
    public static readonly byte[] FAIL = { };
    //    public override bool IsReadable(byte[] data)
    //    {
    //#if UNITY_EDITOR
    //        if (data.Length != 3)
    //        {
    //            Debug.LogWarning($"Invalid messages : {DebugLog.GetStrings(data)}");
    //        }
    //#endif
    //        return data.Length == 3;
    //    }
}
