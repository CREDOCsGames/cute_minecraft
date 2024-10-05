using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Character.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovementComponent : MonoBehaviour
    {
        Rigidbody mRigid;
        Rigidbody Rigid
        {
            get
            {
                if(mRigid == null)
                {
                    mRigid = GetComponent<Rigidbody>();
                }
                return mRigid;
            }
        }
        MovementAction mBeforeAction;
        [SerializeField] MovementAction mDefaultActionOrNull;

        public void PlayMovement(MovementAction movement)
        {
            RemoveMovement();
            mBeforeAction = movement;
            movement.PlayAction(Rigid, this);

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
            if (mDefaultActionOrNull != null)
            {
                PlayMovement(mDefaultActionOrNull);
            }
        }

    }
}