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
        public Character.Character ControlledCharacter
        {
            get => mCurrentController.GetComponentInParent<Character.Character>();
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

        public void Warp(Transform transform)
        {
            if (mDefaultController == null)
            {
                return;
            }
            ControlledCharacter.transform.position = transform.position;
        }

    }
}
