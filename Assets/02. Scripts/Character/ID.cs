using System;
using UnityEngine;

namespace PlatformGame.Character
{
    [Serializable]
    public class ID
    {
        [SerializeField] string mName;
        public string Name
        {
            get
            {
                var name = mName.ToString();
                Debug.Assert(!string.IsNullOrEmpty(name));
                return name;
            }
        }

    }

}
