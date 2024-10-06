using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public static class ListHelper
    {
        public static bool IsOutOfRange<T>(List<T> list, int index)
        {
#if DEVELOPMENT
            if (index < 0 || list.Count <= index)
            {
                Debug.Log($"{index}/{list.Count}");
            }
#endif
            return index < 0 || list.Count <= index;
        }
    }
}