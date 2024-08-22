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
    public UnityEvent Action;
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

    public void ChangeEvent(string key, UnityEvent action)
    {
        var origin = ButtonEvents.FindIndex(x => x.Key.ToString() == key);
        if (origin == -1)
        {
            Debug.Log($"Faild. Not found key : {key}");
            return;
        }
        ButtonEvents[origin].Action = action;
    }

    public UnityEvent GetEvent(string key)
    {
        var origin = ButtonEvents.FindIndex(x => x.Key.ToString() == key);
        Debug.Assert(origin != -1, $"Not found : {key}");
        return ButtonEvents[origin].Action;
    }

    public bool ExitsKey(string key)
    {
        return ButtonEvents.FindIndex(x => x.Key.ToString() == key) != -1;
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

            input.Action.Invoke();
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