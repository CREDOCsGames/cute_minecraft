using System;
using UnityEngine;

namespace PlatformGame.Character
{
    public class StateBaseAnimatorTrigger : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Character mCharacter;
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
                mCharacter.Animator.ResetTrigger(trigger);
            }
            mCharacter.Animator.SetTrigger(state.ToString());
        }

        void OnDestroy()
        {
            mCharacter.OnChangedState.RemoveListener(SendTrigger);
        }
    }

}

