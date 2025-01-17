using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
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
    public class FlowerReader : DataReader
    {
        public const byte EMPTY = 0;
        public const byte FLOWER_RED = 1;
        public const byte FLOWER_GREEN = 2;
        public static readonly byte[] FLOWER_CREATE = { 3 };
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
    public class SystemReader : DataReader
    {
        public static readonly byte[] FAIL = { 0 };
        public static readonly byte[] CLEAR_TOP_FACE = { 1 };
        public static readonly byte[] CLEAR_LEFT_FACE = { 2 };
        public static readonly byte[] CLEAR_FRONT_FACE = { 3 };
        public static readonly byte[] CLEAR_RIGHT_FACE = { 4 };
        public static readonly byte[] CLEAR_BACK_FACE = { 5 };
        public static readonly byte[] CLEAR_BOTTOM_FACE = { 6 };
        public static readonly byte[] ROTATE_CUBE = { 7 };
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

    public class MonsterReader : DataReader
    {
        public static readonly byte[] BOSS_SPAWN = { 0 };
        public static readonly byte[] BOSS_EXIT = { 1 };
        public static readonly byte[] SLIME_SPAWN = { 3 };
        public static readonly byte[] SLIME_BOUNCE = { 4 };
        public static readonly byte[] SLIME_LAND = { 5 };
        public static readonly byte[] BOSS_SPIT_OUT_FAIL = { 7 };
        public static readonly byte[] BOSS_SPIT_OUT_SUCCESS = { 6 };
        public static readonly IDataChecker SLIME_ENTER = new CountChecker((byte)3);
        public static readonly IDataChecker BOSS_SPIT_OUT = new CountChecker((byte)2);
        public override bool IsReadable(byte[] data)
        {
            return data.Length == 1;
        }
        public static bool CreateSlimeSpawnData(byte x, byte z, out byte[] data)
        {
            data = new byte[] { x, z };
            return true;
        }
    }
}