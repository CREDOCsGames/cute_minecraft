using Input1;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;
using static Character.CharacterComponent;

namespace Character
{
    [CreateAssetMenu(menuName = "Custom/PlayerCharacterManager")]
    public class PlayerCharacterManager : UniqueScriptableObject<PlayerCharacterManager>
    {
        public static List<CharacterComponent> JoinCharacters
            => CharacterComponent.Instances.Where(x => x.CompareTag(TAG_PLAYER)).ToList();

        private static List<PlayerControllerComponent> JoinCharactersController
        {
            get
            {
                var playerControllers =
                    PlayerControllerComponent.Instances.Where(x => x.CompareTag(TAG_PLAYER)).ToList();
                if (playerControllers.Count == 0)
                {
                    playerControllers = FindObjectsOfType<PlayerControllerComponent>()
                        .Where(x => x.CompareTag(TAG_PLAYER)).ToList();
                }

                if (playerControllers.Count == 0)
                {
                    Debug.Log($"No controllers with the {TAG_PLAYER} tag found.");
                }

                return playerControllers;
            }
        }

        public CharacterComponent ControlledCharacter
            => _currentController?.GetComponentInParent<CharacterComponent>();

        private PlayerControllerComponent _currentController;
        private PlayerControllerComponent _defaultController;

        public void ControlDefaultCharacter()
        {
            if (JoinCharactersController.Count == 0)
            {
                return;
            }

            if (_defaultController == null)
            {
                JoinCharactersController.ForEach(x => x.SetActive(false));
                _defaultController = JoinCharactersController.First();
            }

            ReplaceControlWith(_defaultController);
        }

        public void ReplaceControlWith(PlayerControllerComponent controller)
        {
            _currentController?.SetActive(false);
            _currentController = controller;
            _currentController.SetActive(true);
        }

        public void SetDefaultCharacter(PlayerControllerComponent controller)
        {
            Debug.Assert(controller.CompareTag(TAG_PLAYER));
            _defaultController = controller;
        }

        public void ReleaseController()
        {
            _currentController?.SetActive(false);
            _currentController = null;
        }

        public void DoAction(ActionData action)
        {
            ControlledCharacter?.DoAction(action);
        }
    }
}