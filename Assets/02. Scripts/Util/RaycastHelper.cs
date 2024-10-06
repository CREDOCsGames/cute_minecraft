using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Util
{
    public static class RaycastHelper
    {
        public static List<Transform> FindObjects(Bounds area)
        {
            return Physics.OverlapBox(area.center, area.extents).Select(x => x.transform).ToList();
        }
    }
}