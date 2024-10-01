using UnityEngine;

namespace Util
{
    public abstract class Condition : MonoBehaviour
    {
        public abstract bool IsTrue();
        public abstract void SetFalse();
    }
}