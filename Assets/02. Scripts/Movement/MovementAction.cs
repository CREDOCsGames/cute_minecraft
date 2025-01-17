using UnityEngine;

namespace Movement
{
    public abstract class MovementAction : ScriptableObject
    {
        public const string ROTATE_ROLL_M90 = "MovementAction/RotateZ+90";
        public const string ROTATE_ROLL_P90 = "MovementAction/RotateZ-90";
        public const string ROTATE_PITCH_M90 = "MovementAction/RotateX+90";
        public const string ROTATE_PITCH_P90 = "MovementAction/RotateX-90";
        public const string ROTATE_YAW_P90 = "MovementAction/RotateY+90";
        public const string ROTATE_YAW_M90 = "MovementAction/RotateY-90";
        public static bool TryGetAction(string path, out MovementAction action)
        {
            action = Resources.Load<MovementAction>(path);
            return action != null;
        }

        public abstract void PlayAction(Rigidbody rigid, MonoBehaviour coroutine);

        public virtual void StopAction(Rigidbody rigid, MonoBehaviour coroutine)
        {
        }

    }
}