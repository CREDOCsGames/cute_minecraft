using System.Collections;
using UnityEngine;

namespace Movement
{
    public abstract class TransformBaseMovement : ScriptableObject
    {
        public abstract IEnumerator Move(Transform start, Transform end, bool repeat = false);
    }

    public abstract class ForceBaseMovement : ScriptableObject
    {
        public abstract IEnumerator Move(Rigidbody rigid);
    }
}