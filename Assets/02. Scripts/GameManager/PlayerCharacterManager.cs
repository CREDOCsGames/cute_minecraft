using PlatformGame.Character.Combat;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static PlatformGame.Character.CharacterComponent;

namespace PlatformGame
{
    [CreateAssetMenu(menuName = "Custom/PlayerCharacterManager")]
    public class PlayerCharacterManager : UniqueScriptableObject<PlayerCharacterManager>
    {
        public List<Character.CharacterComponent> JoinCharacters
        {
            get => Character.CharacterComponent.Instances.Where(x => x.CompareTag(TAG_PLAYER)).ToList();
        }
        public List<PlayerController> JoinCharactersController
        {
            get
            {
                var playerControllers = PlayerController.Instances.Where(x => x.CompareTag(TAG_PLAYER)).ToList();
                if (playerControllers.Count == 0)
                {

                    playerControllers = FindObjectsOfType<PlayerController>().Where(x => x.CompareTag(TAG_PLAYER)).ToList();
                }

                if (playerControllers.Count == 0)
                {
                    Debug.Log($"No controllers with the {TAG_PLAYER} tag found.");
                }
                return playerControllers;
            }
        }
        public Character.CharacterComponent ControlledCharacter
        {

            get
            {
                return mCurrentController?.GetComponentInParent<Character.CharacterComponent>();
            }
        }
        PlayerController mCurrentController;
        PlayerController mDefaultController;
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

        public void ReplaceControlWith(PlayerController controller)
        {
            mCurrentController?.SetActive(false);
            mCurrentController = controller;
            mCurrentController.SetActive(true);
        }

        public void SetDefaultCharacter(PlayerController controller)
        {
            Debug.Assert(controller.tag == TAG_PLAYER);
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
