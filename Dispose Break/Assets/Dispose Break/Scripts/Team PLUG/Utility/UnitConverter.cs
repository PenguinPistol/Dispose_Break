using UnityEngine;

namespace com.TeamPlug.Utility
{
    public class UnitConverter
    {
        public static Vector2   IndexToCoord(int index, int width)      { return new Vector2(index % width, index / width); }
        public static int       CoordToIndex(Vector2 coord, int width)  { return (int)(coord.y * width + coord.x); }

        public static float     DpToPixels(float dp)                    { return dp * (Screen.dpi / 160); }
        public static float     PixelsToDp(float pixels)                { return pixels / (Screen.dpi / 160); }
    }
}

