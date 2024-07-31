using PlatformGame.Character.Combat;
using System.Linq;
using UnityEngine;

namespace PlatformGame.Character
{
    public class CharacterRadar : MonoBehaviour
    {
        public string CharacterName;

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
            Debug.Assert(!string.IsNullOrEmpty(CharacterName));
            var character = Character.Instances.Where(x => x.ID.Name.Equals(CharacterName))?.First();
            Debug.Assert(character != null, $"Not found Character : {CharacterName}");

            return character;
        }



    }
}
