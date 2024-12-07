using UnityEngine;

namespace Movement
{
    public abstract class MovementAction : ScriptableObject
    {
        public abstract void PlayAction(Rigidbody rigid, MonoBehaviour coroutine);

        public virtual void StopAction(Rigidbody rigid, MonoBehaviour coroutine)
        {
        }

    }
}