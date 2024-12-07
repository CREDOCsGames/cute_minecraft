using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Input1
{
    [Serializable]
    public class ButtonEvent
    {
        public ActionKey.Button Key;
        public InputType InputType;
        public UnityEvent Event = new();

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public enum InputType
    {
        Down,
        Up,
        Stay
    }

    public class PlayerController
    {
        private bool _active;
        public float CoolingDuration { get; private set; }
        private List<ButtonEvent> buttonEvents = new();

        public bool IsActive
        {
            get => _active;
            set => _active = value;
        }

        public void AddButtonEvent(ButtonEvent data)
        {
            buttonEvents.Add(data);
        }

        public void RemoveButtonEvent(ButtonEvent data)
        {
            buttonEvents.Remove(data);
        }

        public void Update()
        {
            if (!IsActive)
            {
                return;
            }

            CoolingDuration += Time.deltaTime;

            foreach (var input in buttonEvents)
            {
                Func<string, bool> inputButton;
                switch (input.InputType)
                {
                    case InputType.Down:
                        inputButton = UnityEngine.Input.GetButtonDown;
                        break;
                    case InputType.Up:
                        inputButton = UnityEngine.Input.GetButtonUp;
                        break;
                    case InputType.Stay:
                        inputButton = UnityEngine.Input.GetButton;
                        break;
                    default:
                        Debug.Assert(false, $"Undefined : {input.InputType}");
                        return;
                }

                if (!inputButton(input.Key.ToString()))
                {
                    continue;
                }
                CoolingDuration = 0f;
                input.Event.Invoke();
            }
        }
    }

}