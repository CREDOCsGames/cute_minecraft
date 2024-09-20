using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Character.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovementComponent : MonoBehaviour
    {
        Rigidbody mRigid;
        MovementAction mBeforeAction;
        [SerializeField] MovementAction mDefaultActionOrNull;

        public void PlayMovement(MovementAction movement)
        {
            RemoveMovement();
            mBeforeAction = movement;
            movement.PlayAction(mRigid, this);

        }

        public void RemoveMovement()
        {
            StopAllCoroutines();
            if (!mBeforeAction)
            {
                return;
            }

            mBeforeAction.StopAction(mRigid, this);
        }

        void Awake()
        {
            mRigid = GetComponent<Rigidbody>();
            if (mDefaultActionOrNull != null)
            {
                PlayMovement(mDefaultActionOrNull);
            }
        }

    }
}