namespace Puzzle
{
    public static class SystemMessage
    {
        public static readonly byte[] CLEAR_TOP = { 0 };
        public static readonly byte[] CLEAR_BOTTOM = { 1 };
        public static readonly byte[] CLEAR_LEFT = { 2 };
        public static readonly byte[] CLEAR_RIGHT = { 3 };
        public static readonly byte[] CLEAR_FRONT = { 4 };
        public static readonly byte[] CLEAR_BACK = { 5 };

        public static bool TryGetSystemMessage(byte[] message)
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
