using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public class ScriptableList<T> : ScriptableObject
    {
        [SerializeField] private List<T> Elements = new();
        public T this[int index]
        {
            get
            {
                if (OutOfRange.CheckLine(index, 0, Elements.Count))
                {
                    Debug.Log($"{DM_ERROR.OUT_OF_RANGE} {index}");
                    return default;
                }
                return Elements[index];
            }
        }

    }
}