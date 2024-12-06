using System.Collections.Generic;

namespace Puzzle
{
    public enum Face : byte
    {
        top, left, front, right, back, bottom
    }

    public static class SystemMessage
    {
        public static readonly byte[] CLEAR_TOP = { 0 };
        public static readonly byte[] CLEAR_BOTTOM = { 5 };
        public static readonly byte[] CLEAR_LEFT = { 1 };
        public static readonly byte[] CLEAR_RIGHT = { 3 };
        public static readonly byte[] CLEAR_FRONT = { 2 };
        public static readonly byte[] CLEAR_BACK = { 4 };
        public static List<byte[]> CLEAR_FACE => new List<byte[]>
        {
            CLEAR_TOP,
            CLEAR_LEFT,
            CLEAR_FRONT,
            CLEAR_RIGHT,
            CLEAR_BACK,
            CLEAR_BOTTOM
        };

        public static bool CheckSystemMessage(byte[] message)
        {
            return message.Equals(CLEAR_TOP)
                || message.Equals(CLEAR_BOTTOM)
                || message.Equals(CLEAR_FRONT)
                || message.Equals(CLEAR_BACK)
                || message.Equals(CLEAR_RIGHT)
                || message.Equals(CLEAR_LEFT);
        }
    }
}
