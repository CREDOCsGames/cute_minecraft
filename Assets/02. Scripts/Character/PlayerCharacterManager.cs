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
        public static List<Character.CharacterComponent> JoinCharacters
            => Character.CharacterComponent.Instances.Where(x => x.CompareTag(TAG_PLAYER)).ToList();

        static List<PlayerControllerComponent> JoinCharactersController
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

        public Character.CharacterComponent ControlledCharacter
            => mCurrentController?.GetComponentInParent<Character.CharacterComponent>();

        PlayerControllerComponent mCurrentController;
        PlayerControllerComponent mDefaultController;

        public void ControlDefaultCharacter()
        {
            if (JoinCharactersController.Count == 0)
            {
                return;
            }

            if (mDefaultController == null)
            {
                JoinCharactersController.ForEach(x => x.SetActive(false));
                mDefaultController = JoinCharactersController.First();
            }

            ReplaceControlWith(mDefaultController);
        }

        public void ReplaceControlWith(PlayerControllerComponent controller)
        {
            mCurrentController?.SetActive(false);
            mCurrentController = controller;
            mCurrentController.SetActive(true);
        }

        public void SetDefaultCharacter(PlayerControllerComponent controller)
        {
            Debug.Assert(controller.CompareTag(TAG_PLAYER));
            mDefaultController = controller;
        }

        public void ReleaseController()
        {
            mCurrentController?.SetActive(false);
            mCurrentController = null;
        }

        public void DoAction(ActionData action)
        {
            ControlledCharacter?.DoAction(action);
        }
    }
}