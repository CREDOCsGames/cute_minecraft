using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedObjectList : SharedVariable<List<Object>>
    {
        public static implicit operator SharedObjectList(List<Object> value) { return new SharedObjectList { mValue = value }; }
    }
}