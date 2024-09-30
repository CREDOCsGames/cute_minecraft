using PlatformGame.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
class ButtonData
{
    public ActionKey.Button Key;
    public InputType InputType;
    public UnityEvent Event;
}

enum InputType
{
    Down, Up, Stay
}

public class PlayerController : MonoBehaviour
{
    static List<PlayerController> mInstances = new();
    public static List<PlayerController> Instances => mInstances.ToList();

    [Header("Controls")]
    [SerializeField] bool mIsActive;
    [SerializeField] List<ButtonData> ButtonEvents;

    public bool IsActive
    {
        get => mIsActive;
        private set => mIsActive = value;
    }

    public void SetActive(bool able)
    {
        IsActive = able;
    }

    public void AddEventListener(string key, UnityAction action)
    {
        if (TryGetKeyIndex(key, out var index))
        {
            ButtonEvents[index].Event.AddListener(action);
        }
    }

    public void RemoveEventListener(string key, UnityAction action)
    {
        if (TryGetKeyIndex(key, out var index))
        {
            ButtonEvents[index].Event.RemoveListener(action);
        }
    }

    public void ChangeEvent(string key, UnityEvent e)
    {
        if (TryGetKeyIndex(key, out int index))
        {
            ButtonEvents[index].Event = e;
        }
    }

    bool TryGetKeyIndex(string key, out int index)
    {
        index = ButtonEvents.FindIndex(x => x.Key.ToString() == key);
        return index != -1;
    }

    void Update()
    {
        if (!IsActive)
        {
            return;
        }

        foreach (var input in ButtonEvents)
        {
            Func<string, bool> inputButton;
            switch (input.InputType)
            {
                case InputType.Down:
                    inputButton = Input.GetButtonDown;
                    break;
                case InputType.Up:
                    inputButton = Input.GetButtonUp;
                    break;
                case InputType.Stay:
                    inputButton = Input.GetButton;
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

    void Awake()
    {
        mInstances.Add(this);
    }

    void OnDestroy()
    {
        mInstances.Remove(this);
    }
}