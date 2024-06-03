using PlatformGame.Character.Combat;
using PlatformGame.Input;
using PlatformGame.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace PlatformGame.Character.Controller
{
    [Serializable]
    public class ActionDataKeyPair
    {
        public string Key;
        public ActionData ActionData;
    }

    public class ControllerInputData
    {
        public readonly string Key;
        public readonly ActionData ActionData;
        public readonly PlayerCharacterController Controller;

        public ControllerInputData(string key, ActionData actionData, PlayerCharacterController controller)
        {
            Key = key;
            ActionData = actionData;
            Controller = controller;
        }
    }

    public class PlayerCharacterController : MonoBehaviour
    {
        static List<PlayerCharacterController> mInstances = new();
        public static List<PlayerCharacterController> Instances => mInstances.ToList();

        [FormerlySerializedAs("EditorInputMap")]
        [HideInInspector] public List<ActionDataKeyPair> EditorInputMap;
        [HideInInspector] public Character EditorBeforeCharacter;

        [Header("References")]
        public Character ControlledCharacter;

        [Header("Controls")]
        [SerializeField] bool mIsActive;
        public bool IsActive
        {
            get => mIsActive;
            private set => mIsActive = value;
        }
        [SerializeField] UnityEvent<ControllerInputData> InputEvents;

        Dictionary<string, ActionData> mInputMap;
        Pipeline<ControllerInputData> mInputPipeline;

        public void SetActive(bool able)
        {
            IsActive = able;
            FocusOn(able);
        }

        void SetInputAction(List<ActionDataKeyPair> actionKeys)
        {
            mInputMap = new Dictionary<string, ActionData>();
            foreach (var item in actionKeys)
            {
                Debug.Assert(!mInputMap.ContainsKey(item.Key), $"duplicate elements in {name} : {item.Key}");
                mInputMap.Add(item.Key, item.ActionData);
            }
        }

        void EnterCommand(ControllerInputData input)
        {
            var actionID = input.ActionData.ID;
            ControlledCharacter.DoAction(actionID);
        }

        void FocusOn(bool on)
        {
            if (!ControlledCharacter.UI)
            {
                return;
            }

            ControlledCharacter.UI.SetActive(on);
        }

        void Awake()
        {
            SetInputAction(EditorInputMap);

            mInputPipeline = Pipelines.Instance.PlayerCharacterControllerPipeline;
            mInputPipeline.InsertPipe(EnterCommand);
            mInputPipeline.InsertPipe((x) => InputEvents.Invoke(x));
            mInstances.Add(this);
        }

        void Update()
        {
            if (!IsActive)
            {
                return;
            }

            var inputMap = ActionKey.GetKeyDownMap();
            foreach (var input_Action in mInputMap)
            {
                var input = inputMap[input_Action.Key];
                if (!input)
                {
                    continue;
                }

                var inputData = new ControllerInputData(input_Action.Key, input_Action.Value, this);
                mInputPipeline.Invoke(inputData);
            }
        }

        void OnDestroy()
        {
            mInstances.Remove(this);
        }

    }
}