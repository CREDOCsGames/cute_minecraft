using System;

namespace Puzzle
{
    public enum Face : byte
    {
        top, left, front, right, back, bottom
    }
    [Flags]
    public enum FaceFlags : byte
    {
        None = 0,
        top = 1 << 0,
        left = 1 << 1,
        front = 1 << 2,
        right = 1 << 3,
        back = 1 << 4,
        bottom = 1 << 5
    }

}
