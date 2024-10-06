using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Input1
{
    public class PlayerControllerComponent : MonoBehaviour
    {
        static readonly List<PlayerControllerComponent> mInstances = new();
        public static IEnumerable<PlayerControllerComponent> Instances => mInstances.ToList();
        PlayerController mController = new();
        [Header("[Options]")][SerializeField] bool mIsActive;
        [SerializeField] List<ButtonEvent> ButtonEvents;

        public bool IsActive
        {
            get => mController.IsActive;
            private set => mController.IsActive = value;
        }

        public void SetActive(bool able)
        {
            IsActive = able;
        }

        void Update()
        {
            mController.Update();
        }

        void Awake()
        {
            foreach (var buttonEvent in ButtonEvents)
            {
                mController.AddButtonEvent(buttonEvent);
            }

            mInstances.Add(this);
        }

        void OnDestroy()
        {
            mInstances.Remove(this);
        }
    }
}