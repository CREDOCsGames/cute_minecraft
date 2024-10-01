using System;
using UnityEngine;

namespace Character
{
    public class StateBaseAnimatorHandler : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        CharacterComponent mCharacter;

        [SerializeField] Animator mAnimator;
        string[] mTriggers;

        void Awake()
        {
            Debug.Assert(mCharacter != null, $"Not found Character : {name}");
            mCharacter.OnChangedState.AddListener(SendTrigger);
            mTriggers = Enum.GetNames(typeof(CharacterState));
        }

        void SendTrigger(CharacterState state)
        {
            foreach (var trigger in mTriggers)
            {
                mAnimator.ResetTrigger(trigger);
            }

            mAnimator.SetTrigger(state.ToString());
        }

        void OnDestroy()
        {
            mCharacter.OnChangedState.RemoveListener(SendTrigger);
        }
    }
}