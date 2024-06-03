using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedGameObjectList : SharedVariable<List<GameObject>>
    {
        public SharedGameObjectList()
        {
            mValue = new List<GameObject>();
        }

        public static implicit operator SharedGameObjectList(List<GameObject> value) { return new SharedGameObjectList { mValue = value }; }
    }
}