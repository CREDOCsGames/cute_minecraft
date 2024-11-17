using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Input1
{
    public class PlayerControllerComponent : MonoBehaviour
    {
        private static readonly List<PlayerControllerComponent> mInstances = new();
        public static IEnumerable<PlayerControllerComponent> Instances => mInstances.ToList();
        private PlayerController _controller = new();
        [Header("[Options]")][SerializeField] private bool _activeOnAwake;
        [SerializeField] private List<ButtonEvent> _buttonEvents;

        public bool IsActive
        {
            get => _controller.IsActive;
            private set => _controller.IsActive = value;
        }

        public void SetActive(bool able)
        {
            IsActive = able;
        }

        private void Update()
        {
            _controller.Update();
        }

        private void Awake()
        {
            foreach (var buttonEvent in _buttonEvents)
            {
                _controller.AddButtonEvent(buttonEvent);
            }

            mInstances.Add(this);
            _controller.IsActive = _activeOnAwake;
        }

        private void OnDestroy()
        {
            mInstances.Remove(this);
        }
    }
}