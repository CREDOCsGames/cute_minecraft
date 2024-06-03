#if DEVELOPMENT
using System.Collections.Generic;
using UnityEngine;

public class DebugGizmos : MonoBehaviour
{
    static readonly Dictionary<int, DebugGizmos> mInstances = new Dictionary<int, DebugGizmos>();
    static Transform mRoot;
    Vector3 mCenter;
    Vector3 mSize;

    public static void DrawWireCube(Vector3 center, Vector3 size, int instanceHash)
    {
        if (mRoot == null)
        {
            mRoot = new GameObject().transform;
            mRoot.name = "DebugGizmos";
        }

        if (!mInstances.ContainsKey(instanceHash))
        {
            var obj = new GameObject("Gizmo");
            obj.transform.SetParent(mRoot);
            var instance = obj.AddComponent<DebugGizmos>();
            mInstances.Add(instanceHash, instance);
        }

        var debugGizmo = mInstances[instanceHash];
        debugGizmo.mCenter = center;
        debugGizmo.mSize = size;
    }

    void OnDrawGizmos()
    {
        if (mSize != Vector3.zero)
        {
            Gizmos.DrawWireCube(mCenter, mSize);
        }
    }
}
#endif