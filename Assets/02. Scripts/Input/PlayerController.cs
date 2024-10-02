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
        public UnityEvent Event;

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
        bool mbActive;
        List<ButtonEvent> mButtonEvents = new();

        public bool IsActive
        {
            get => mbActive;
            set => mbActive = value;
        }

        public void AddButtonEvent(ButtonEvent data)
        {
            mButtonEvents.Add(data);
        }

        public void RemoveButtonEvent(ButtonEvent data)
        {
            mButtonEvents.Remove(data);
        }

        public void Update()
        {
            if (!IsActive)
            {
                return;
            }

            foreach (var input in mButtonEvents)
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

                input.Event.Invoke();
            }
        }
    }

}