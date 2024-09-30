using UnityEngine;

namespace PlatformGame.Character.Movement
{
    public abstract class MovementAction : ScriptableObject
    {
        Rigidbody mRigid;
        Coroutine mCoroutine;

        public abstract void PlayAction(Rigidbody rigid, MonoBehaviour coroutine);

        public virtual void StopAction(Rigidbody rigid, MonoBehaviour coroutine)
        {
        }

    }
}