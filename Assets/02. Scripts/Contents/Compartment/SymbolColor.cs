namespace PlatformGame.Contents.Compartment
{
    public enum Color
    {
        None, Yellow, Green, Blue, Orange, Red, White
    }

    public static class SymbolColor
    {
        static readonly UnityEngine.Color NONE = UnityEngine.Color.magenta;
        static readonly UnityEngine.Color YELLOW = UnityEngine.Color.yellow;
        static readonly UnityEngine.Color GREEN = UnityEngine.Color.green;
        static readonly UnityEngine.Color BLUE = UnityEngine.Color.blue;
        static readonly UnityEngine.Color ORANGE = new UnityEngine.Color(255f / 255f, 165f / 255f, 0f, 255 / 255f);
        static readonly UnityEngine.Color RED = UnityEngine.Color.red;
        static readonly UnityEngine.Color WHITE = UnityEngine.Color.white;

        public static readonly UnityEngine.Color[] ColorArray =
        {
            NONE,
            YELLOW,
            GREEN,
            BLUE,
            ORANGE,
            RED,
            WHITE,
        };
    }

}