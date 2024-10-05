using PlatformGame.Character.Combat;
using UnityEngine;

namespace PlatformGame.Character
{
    public class CharacterRadar : MonoBehaviour
    {
        public void DoAction(ActionData action)
        {
            var character = FindCharacter();
            character.DoAction(action.ID);
        }

        public void SelectAndDoAction(ActionData action)
        {
            var character = FindCharacter();
            character.DoAction(action.ID);
        }

        public void ChangeAnimator(RuntimeAnimatorController controller)
        {
            var character = FindCharacter();
            character.Animator.runtimeAnimatorController = controller;
        }

        Character FindCharacter()
        {
            var character = PlayerCharacterManager.Instance.ControlledCharacter;
            Debug.Assert(character != null);
            return character;
        }



    }
}
