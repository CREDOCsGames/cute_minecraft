using PlatformGame.Character.Collision;
using PlatformGame.Character.Combat;
using PlatformGame.Character.Movement;
using PlatformGame.Debugger;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Character
{
    public class Character : MonoBehaviour
    {
        public const string TAG_PLAYER = "Player";
        static readonly List<Character> mInstances = new();
        public static List<Character> Instances => mInstances.ToList();
        public bool IsAction => mAgent.IsAction;

        public Func<bool> IsGrounded
        {
            get; set;
        }
        CharacterState mState;
        public CharacterState State
        {
            get => mState;
            private set
            {
                if (mState != value)
                {
                    OnChangedState.Invoke(value);
                    DebugLog.PrintLog(transform, value);
                }
                mState = value;
            }
        }

        [Header("References")]
        [SerializeField] GameObject mUI;
        [SerializeField] HitBoxGroup mHitBox;
        public GameObject UI => mUI;
        [SerializeField] Rigidbody mRigid;
        public Rigidbody Rigid => mRigid;
        [SerializeField] MovementComponent mMovement;
        public MovementComponent Movement => mMovement;
        [SerializeField] Transform mModel;
        public Transform Model => mModel;


        [Header("Controls")]
        [SerializeField] ActionDataList mHasAbilities;
        public ActionDataList HasAbilities => mHasAbilities;
        [SerializeField] AttributeFlag mAttribute;
        public AttributeFlag Attribute => mAttribute;
        [SerializeField] UnityEvent<CharacterState> mOnChangedState;
        public UnityEvent<CharacterState> OnChangedState => mOnChangedState;

        AbilityAgent mAgent;

        public void ReleaseRest()
        {
            DoAction(4290000009);
            Rigid.isKinematic = false;
            UI.SetActive(true);
        }

        public void Rest()
        {
            DoAction(4290000008);
            Rigid.isKinematic = true;
            Rigid.velocity = Vector3.zero;
            UI.SetActive(false);
        }

        public void DoAction(uint actionID)
        {
            mHasAbilities.Library.TryGetValue(actionID, out var action);
           Debug.Assert(action, $"The {actionID} is not registered as an ability for {gameObject.name}.");

            if (!StateCheck.Equals(State, action.AllowedState))
            {
                return;
            }
            State = action.BeState;

            mAgent.UseAbility(action);

            if (!action.Movement)
            {
                return;
            }
            Movement.PlayMovement(action.Movement);
        }

        void Awake()
        {
            Debug.Assert(mHitBox, $"HitBox reference not found : {gameObject.name}");
            Debug.Assert(Rigid, $"Rigidbody reference not found : {gameObject.name}");
            Debug.Assert(mMovement, $"Movement reference not found : {gameObject.name}");
            Debug.Assert(mModel, $"Model reference not found : {gameObject.name}");
            mInstances.Add(this);
            mAgent = new AbilityAgent(mHitBox);
            Attribute.SetFlag(Attribute.Flags, this);

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