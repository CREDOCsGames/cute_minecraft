using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Util
{
    [Serializable]
    public class CustomList<T>
    {
        public List<T> List = new();
    }

    public class ScriptableMatrix<T> : ScriptableObject
    {
        [SerializeField] List<CustomList<T>> mMatrix = new();
        public List<CustomList<T>> Matrix => mMatrix;
    }

}
