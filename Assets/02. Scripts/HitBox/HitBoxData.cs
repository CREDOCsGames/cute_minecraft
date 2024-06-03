using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlatformGame.Character.Collision
{
    [Serializable]
    public class HitBoxData
    {
        [SerializeField] List<string> mHitBoxNames;

        public List<string> Filter
        {
            get
            {
#if UNITY_EDITOR
                List<string> duplicates = new();
                foreach (var name in mHitBoxNames)
                {
                    Debug.Assert(!duplicates.Contains(name), $"Duplicate value : {name}");
                    duplicates.Add(name);
                }
#endif
                return mHitBoxNames.ToList();
            }
        }

        public bool UseHitBox => Filter.Count > 0;
    }
}