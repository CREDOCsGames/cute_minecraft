using System;
using UnityEngine;

namespace PlatformGame.Mamager
{
    [Flags]
    public enum Side
    {
        right = 1 << 0,
        left = 1 << 1,
        forward = 1 << 2,
        backward = 1 << 3,
        up = 1 << 4,
        down = 1 << 5
    }

    public class AreaWall
    {
        Side mSide;
        Bounds mBounds;

        public AreaWall(Side side, Bounds bounds)
        {
            mSide = side;
            mBounds = bounds;
        }

        public void Create()
        {

        }

        public void Destroy()
        {

        }
    }

}
