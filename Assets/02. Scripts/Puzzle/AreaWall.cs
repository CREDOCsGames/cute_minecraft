using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Puzzle
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
        readonly Side _side;
        Bounds _bounds;
        string _path;

        static readonly Vector3[] dirs =
        {
            new Vector3(1 / 2f, 0, 0),
            new Vector3(0, 0, -1 / 2f),
            new Vector3(-1 / 2f, 0, 0),
            new Vector3(0, 0, 1 / 2f),
            new Vector3(0, 1 / 2f, 0),
            new Vector3(0, -1 / 2f, 0)
        };

        static readonly Vector3[] scales =
        {
            new Vector3(0, 1, 1),
            new Vector3(1, 1, 0),
            new Vector3(0, 1, 1),
            new Vector3(1, 1, 0),
            new Vector3(1, 0, 1),
            new Vector3(1, 0, 1)
        };

        public List<GameObject> Objects { get; private set; } = new();

        public AreaWall(Side side, Bounds bounds, string wall)
        {
            _side = side;
            _bounds = bounds;
            _path = wall;
        }

        public void Create()
        {
            var center = _bounds.center;
            var size = _bounds.size;
            var wallThickness = Vector3.one * 0.1f;

            for (var i = 0; i < 6; i++)
            {
                if (!_side.HasFlag((Side)(1 << i)))
                {
                    continue;
                }

                var position = center + Vector3.Scale(dirs[i], wallThickness + size);
                var wall = Object.Instantiate(Resources.Load<GameObject>(_path + '_' + (Side)(1 << i)));
                wall.name = $"Wall_{(Side)(1 << i)}";
                wall.transform.position = position;
                wall.transform.localScale = Vector3.Scale(scales[i], size) + Vector3.one * 0.1f;
                Objects.Add(wall);
            }
        }

        public void SetWall(string wall)
        {
            _path = wall;
        }

        public void Destroy()
        {
            foreach (var wall in Objects.ToList())
            {
                Object.Destroy(wall);
                Objects.Remove(wall);
            }
        }
    }
}