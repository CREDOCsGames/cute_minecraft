using PlatformGame.Character.Controller;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static PlatformGame.Character.Character;

namespace PlatformGame
{
    [CreateAssetMenu(menuName = "Custom/PlayerCharacterManager")]
    public class PlayerCharacterManager : UniqueScriptableObject<PlayerCharacterManager>
    {
        public List<Character.Character> JoinCharacters
        {
            get => Character.Character.Instances.Where(x => x.CompareTag(TAG_PLAYER)).ToList();
        }
        public List<ActionController> JoinCharactersController
        {
            get
            {
                var playerControllers = ActionController.Instances.Where(x => x.CompareTag(TAG_PLAYER)).ToList();
                if (playerControllers.Count == 0)
                {
                    Debug.Log($"No controllers with the {TAG_PLAYER} tag found.");
                }
                return playerControllers;
            }
        }
        public Character.Character ControlledCharacter
        {
            get => mCurrentController.ControlledCharacter;
        }
        ActionController mCurrentController;

        public void ControlDefaultCharacter()
        {
            if (JoinCharactersController.Count == 0)
            {
                return;
            }
            JoinCharactersController.ForEach(x => x.SetActive(false));
            var defaultCharacter = JoinCharactersController.First();
            ReplaceControlWith(defaultCharacter);
        }

        public void ReplaceControlWith(ActionController controller)
        {
            mCurrentController?.SetActive(false);
            mCurrentController = controller;
            mCurrentController.SetActive(true);
        }

        public void ReleaseController()
        {
            mCurrentController?.SetActive(false);
            mCurrentController = null;
        }

    }
}
