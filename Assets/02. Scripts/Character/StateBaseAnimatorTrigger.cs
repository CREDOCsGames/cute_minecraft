using UnityEngine;

namespace PlatformGame.Character
{
    public class StateBaseAnimatorTrigger : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Character mCharacter;

        void Awake()
        {
            Debug.Assert(mCharacter != null,$"Not found Character : {name}");
            mCharacter.OnChangedState.AddListener(SendTrigger);
        }

        void SendTrigger(CharacterState state)
        {
            mCharacter.Animator.SetTrigger(state.ToString());
        }

        void OnDestroy()
        {
            mCharacter.OnChangedState.RemoveListener(SendTrigger);
        }
    }

}

