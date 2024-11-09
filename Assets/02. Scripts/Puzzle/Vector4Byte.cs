using UnityEngine;

namespace Puzzle
{
    public struct Vector4Byte
    {
        public static readonly Vector4Byte FAIL = new(255, 255, 255, 255);
        public static readonly Vector4Byte END = new(255, 255, 255, 254);
        public byte x;
        public byte y;
        public byte z;
        public byte w;

        public Vector4Byte(byte x, byte y, byte z, byte w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public bool Equals(byte[] obj)
        {
            if (obj.Length != 4) return false;
            return obj[0].Equals(x)
                && obj[1].Equals(y)
                && obj[2].Equals(z)
                && obj[3].Equals(w);
        }

        public static byte[] Convert2ByteArr(Vector4Byte vector4Byte)
        {
            return new[] { vector4Byte.x, vector4Byte.y, vector4Byte.z, vector4Byte.w };
        }

        public static Vector4Byte Convert2Vector4Byte(byte[] data)
        {
            Debug.Assert(data.Length == 4);
            return new Vector4Byte(data[0], data[1], data[2], data[3]);
        }
    }

}
