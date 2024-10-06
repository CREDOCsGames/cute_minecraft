using UnityEngine;

namespace Movement
{
    public static class MovementHelper
    {
        public static bool IsNearByDistance(Vector3 a, Vector3 b, float near = 0.1f)
        {
            return Vector3.Distance(a, b) <= near;
        }

        public static bool IsNearByDistance(Transform a, Transform b, float near = 0.1f)
        {
            return IsNearByDistance(a.position, b.position, near);
        }
    }
}