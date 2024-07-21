namespace PlatformGame.Contents.Compartment
{
    public enum Color
    {
        None, Yellow, Green, Blue, Orange, Red, White
    }

    public static class SymbolColor
    {
        public static readonly UnityEngine.Color NONE = UnityEngine.Color.magenta;


        public static readonly UnityEngine.Color YELLOW = UnityEngine.Color.yellow;
        public static readonly UnityEngine.Color GREEN = UnityEngine.Color.green;
        public static readonly UnityEngine.Color BLUE = UnityEngine.Color.blue;
        public static readonly UnityEngine.Color ORANGE = new UnityEngine.Color(255f / 255f, 165f / 255f, 0f, 255 / 255f);
        public static readonly UnityEngine.Color RED = UnityEngine.Color.red;
        public static readonly UnityEngine.Color WHITE = UnityEngine.Color.white;

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