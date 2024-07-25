using PlatformGame.Character.Combat;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Character
{
    public class BlockEvent : MonoBehaviour
    {
        public ActionData ButtonAction;
        public UnityEvent ButtonEvent;

        

        public void SetGrounded(Character character)
        {
            character.IsGrounded = () => true;
        }

        public void SetJumping(Character character)
        {
            character.IsGrounded = () => false;
        }

        void Awake()
        {
            ButtonEvent.AddListener(() =>Destroy(gameObject.GetComponent<BlockEvent>()));
        }

    }
}