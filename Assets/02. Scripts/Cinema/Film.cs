using UnityEngine;
using Util;

namespace Cinema
{
    public class Film
    {
        public readonly string Name;
        public readonly float Time;
        public bool IsBad { get; private set; }
        public Film(string name, float time)
        {
            Name = name;
            Time = time;
            IsBad = SceneManagerUtil.IsSceneUnvalid(name);
            Debug.Assert(!IsBad, $"Please add the scene to your build settings : {name}");
        }

        public static readonly Film EMPTY = new Film("EMPTY_FILM", 0f);
    }
}
