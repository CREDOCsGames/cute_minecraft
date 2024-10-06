using PlatformGame.Character.Combat;
using PlatformGame.Character.Movement;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Character
{
    public class CharacterComponent : MonoBehaviour
    {
        public const string TAG_PLAYER = "Player";
        static readonly List<CharacterComponent> mInstances = new();
        public static List<CharacterComponent> Instances => mInstances.ToList();
        public bool IsAction => mCooltime.IsAction;
        public Func<bool> IsGrounded
        {
            get; set;
        } = () => true;
        CharacterState mState;
        public CharacterState State
        {
            get => mState;
            private set
            {
                if (mState != value)
                {
                    OnChangedState.Invoke(value);
                }
                mState = value;
            }
        }

        [SerializeField] Rigidbody mRigid;
        public Rigidbody Rigid => mRigid;
        [SerializeField] Animator mAnimator;
        public Animator Animator => mAnimator;

        [SerializeField] MovementComponent mMovement;
        public MovementComponent Movement => mMovement;
        readonly Cooltime mCooltime = new();

        [Header("Options")]
        [SerializeField] ActionDataList mHasAbilities;
        public ActionDataList HasAbilities => mHasAbilities;
        [SerializeField] UnityEvent<CharacterState> mOnChangedState;
        public UnityEvent<CharacterState> OnChangedState => mOnChangedState;

        public void DoAction(ActionData data)
        {
            DoAction(data.ID);
        }

        public void DoAction(int actionID)
        {
            mHasAbilities.Library.TryGetValue(actionID, out var action);
            Debug.Assert(action, $"The {actionID} is not registered as an ability for {gameObject.name}.");

            if (!StateCheck.Equals(State, action.AllowedState))
            {
                return;
            }
            State = action.BeState;

            mCooltime.UseAbility(action);

            if (!action.Movement)
            {
                return;
            }
            Movement.PlayMovement(action.Movement);
        }

        public void ResetAction()
        {
            mCooltime.Reset();
        }

        void Awake()
        {
            Debug.Assert(Rigid, $"Rigidbody reference not found : {gameObject.name}");
            Debug.Assert(mMovement, $"Movement reference not found : {gameObject.name}");
            mInstances.Add(this);
        }

        void Start()
        {
            OnChangedState.Invoke(State);
        }

        void OnDestroy()
        {
            mInstances.Remove(this);
        }

    }
}