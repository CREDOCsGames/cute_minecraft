using System;
using UnityEngine;
using Util;

namespace Cinema
{
    [Serializable]
    public class Film
    {
        public static readonly Film EMPTY = new("EMPTY_FILM", 3f);
        public bool IsBad { get; private set; }
        public readonly float Time;
        public readonly string Name;
        public Film(string name, float time)
        {
            Name = name;
            Time = Mathf.Max(0, time);
            IsBad = SceneManagerUtil.IsSceneUnvalid(name);
#if DEVELOPMENT
            if (SceneManagerUtil.IsSceneUnvalid(name))
            {
                Debug.LogWarning($"Please add the scene to your build settings : {name}");
            }
            if (time < 0)
            {
                Debug.LogWarning($"Duration must be greater than 0 : {time}");
            }
#endif
        }
    }
}