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
                    
                    playerControllers = FindObjectsOfType<ActionController>().Where(x => x.CompareTag(TAG_PLAYER)).ToList();
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
            get => mCurrentController.ControlledCharacter;
        }
        ActionController mCurrentController;
        ActionController mDefaultController;
        public void ControlDefaultCharacter()
        {
            if (JoinCharactersController.Count == 0)
            {
                return;
            }
            if(mDefaultController == null)
            {
                JoinCharactersController.ForEach(x => x.SetActive(false));
                mDefaultController = JoinCharactersController.First();
            }
            
            ReplaceControlWith(mDefaultController);
        }

        public void ReplaceControlWith(ActionController controller)
        {
            mCurrentController?.SetActive(false);
            mCurrentController = controller;
            mCurrentController.SetActive(true);
        }

        public void SetDefaultCharacter(ActionController controller)
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
            if(mDefaultController == null)
            {
                return;
            }
            mDefaultController.ControlledCharacter.transform.position = transform.position;
        }

    }
}
