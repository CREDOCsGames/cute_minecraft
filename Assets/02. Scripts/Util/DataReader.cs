using System.Collections.Generic;
using System.Linq;
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
        public static FlowerReader _instance;
        public static FlowerReader Instance
        {
            get
            {
                _instance ??= new FlowerReader();
                return _instance;
            }
        }
        public const byte NONE = 0;
        public const byte FLOWER_RED = 1;
        public const byte FLOWER_GREEN = 2;
        public const byte EVENT_CROSS = 1;
        public const byte EVENT_DOT = 2;
        public const byte EVENT_CREATE = 3;
        public const Face BOSS_STAGE = Face.bottom;
        public const Face SHELTER = Face.bottom;
        public static IDataChecker FLOWER_CREATE = new CountChecker((byte)4);

        public static bool IsFlower(byte data)
        {
            return data == FLOWER_RED || data == FLOWER_GREEN;
        }
        public static byte GetFace(byte[] data)
        {
            if (data.Length < 3)
            {
                Debug.LogError($" {DM_ERROR.INVALID_FORMAT} : {data}");
                return 255;
            }
            return data[2];
        }
        public static byte[] CreateHitMessage(byte[] index)
        {
            return index;
        }
        public static byte[] AdditiveMessage(byte[] message, byte attackType)
        {
            if(message.Length == 3)
            {
                message = message.Concat(new byte[1]).ToArray();
            }
            message[3] = attackType;
            return message;
        }
        public static byte[] GetFlowerIndex(byte[] data)
        {
            return data;
        }
        public static byte GetFlowerColor(byte[] data)
        {
            if (data.Length < 4)
            {
                Debug.LogError($" {DM_ERROR.INVALID_FORMAT} : {data}");
                return 255;
            }
            return data[3];
        }
        public static byte GetEventData(byte[] data)
        {
            if (data.Length < 4)
            {
                return NONE;
            }

            return data[3];
        }
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
        private static SystemReader _instance;
        public static SystemReader Instance
        {
            get
            {
                _instance ??= new SystemReader();
                return _instance;
            }
        }
        public static readonly byte[] FAIL = { 0 };
        public static readonly byte[] CLEAR_TOP_FACE = { 1 };
        public static readonly byte[] CLEAR_LEFT_FACE = { 2 };
        public static readonly byte[] CLEAR_FRONT_FACE = { 3 };
        public static readonly byte[] CLEAR_RIGHT_FACE = { 4 };
        public static readonly byte[] CLEAR_BACK_FACE = { 5 };
        public static readonly byte[] CLEAR_BOTTOM_FACE = { 6 };
        public static readonly byte[] ROTATE_CUBE = { 7 };
        public static readonly byte[] READY_PLAYER = { 8 };
        private static readonly List<byte[]> CLEAR_MESSAGES = new() { CLEAR_TOP_FACE, CLEAR_LEFT_FACE, CLEAR_FRONT_FACE, CLEAR_RIGHT_FACE, CLEAR_BACK_FACE, CLEAR_BOTTOM_FACE };
        public static byte[] GetClearMessage(Face face)
        {
            return CLEAR_MESSAGES[(byte)face];
        }
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
        public static MonsterReader _instance;
        public static MonsterReader Instance
        {
            get
            {
                _instance ??= new MonsterReader();
                return _instance;
            }
        }
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