using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlatformGame.Mamager
{
    [Flags]
    public enum Side
    {
        right = 1 << 0,
        backward = 1 << 1,
        left = 1 << 2,
        forward = 1 << 3,
        up = 1 << 4,
        down = 1 << 5
    }

    public class AreaWall
    {
        Side mSide;
        Bounds mBounds;
        string mPath;
        static readonly Vector3[] dirs =
        {
                new Vector3(1/2f,0,0),
                new Vector3(0,0,-1/2f),
                new Vector3(-1/2f,0,0),
                new Vector3(0,0,1/2f),
                new Vector3(0,1/2f,0),
                new Vector3(0,-1/2f,0)
        };
        static readonly Vector3[] scales =
        {
            new Vector3(0,1,1),
            new Vector3(1,1,0),
            new Vector3(0,1,1),
            new Vector3(1,1,0),
            new Vector3(1,0,1),
            new Vector3(1,0,1)
        };
        public List<GameObject> Objects { get; private set; } = new();

        public AreaWall(Side side, Bounds bounds, string wall)
        {
            mSide = side;
            mBounds = bounds;
            mPath = wall;
        }

        public void Create()
        {

            var center = mBounds.center;
            var size = mBounds.size;
            var wallThickness = Vector3.one * 0.1f;

            for (int i = 0; i < 6; i++)
            {
                if (mSide.HasFlag((Side)(1 << i)))
                {
                    var position = center + Vector3.Scale(dirs[i], wallThickness + size);
                    var wall = GameObject.Instantiate(Resources.Load<GameObject>(mPath + '_' + (Side)(1 << i)));
                    wall.name = $"Wall_{(Side)(1 << i)}";
                    wall.transform.position = position;
                    wall.transform.localScale = Vector3.Scale(scales[i], size) + Vector3.one * 0.1f;
                    Objects.Add(wall);
                }
            }

        }

        public void SetWall(string wall)
        {
            mPath = wall;
        }

        public void Destroy()
        {
            foreach (var wall in Objects.ToList())
            {
                GameObject.Destroy(wall);
                Objects.Remove(wall);
            }
        }
    }

}
