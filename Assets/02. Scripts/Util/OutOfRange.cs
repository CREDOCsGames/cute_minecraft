namespace Util
{
    public static class OutOfRange
    {
        public static bool CheckLine(int value, int min, int max)
        {
            return value < min || max < value;
        }

        public static bool CheckBox(int x, int y, int width, int height)
        {
            return x < 0 || width <= x || y < 0 || height <= y;
        }
    }
}
