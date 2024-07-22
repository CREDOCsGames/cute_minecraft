using UnityEngine;
using static PlatformGame.Character.CharacterStateFlags;

namespace PlatformGame.Character
{
    public class PatrolPet : Role
    {
        [SerializeField] Character mCharacter;

        CharacterState mBeforeState;
        bool mbChangedJumpState;
        bool mbChangedIdleState;
        void OnChangeState(CharacterState state)
        {
            if (state is CharacterState.Falling)
            {
                return;
            }

            mbChangedJumpState =
                (StateCheck.Equals(state, Jump) &&
                !StateCheck.Equals(mBeforeState, Jump));

            if (mbChangedJumpState)
            {
                SwapTransform();
            }

            mbChangedIdleState =
                !StateCheck.Equals(state, Jump) &&
                StateCheck.Equals(mBeforeState, Jump);
            if (mbChangedIdleState)
            {
                SwapTransform();
            }

            mBeforeState = state;
        }

        protected override void Awake()
        {
            base.Awake();
            mSwapTransform = mCharacter.transform;
            mCharacter.OnChangedState.AddListener(OnChangeState);
        }

        void OnDestroy()
        {
            mCharacter.OnChangedState.RemoveListener(OnChangeState);
        }

    }
}