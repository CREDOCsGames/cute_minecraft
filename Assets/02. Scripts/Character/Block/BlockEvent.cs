using PlatformGame.Character.Combat;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Character
{
    public class BlockEvent : MonoBehaviour
    {
        public ActionData ButtonAction;
        public UnityEvent ButtonEvent;
        public bool IsEnable { get; private set; } = true;

        void Awake()
        {
            ButtonEvent.AddListener(RemoveComponent);
        }

        public void SetEnable(bool enable)
        {
            IsEnable = enable;
        }

        void RemoveComponent()
        {
            Destroy(gameObject.GetComponent<BlockEvent>());
        }
    }
}