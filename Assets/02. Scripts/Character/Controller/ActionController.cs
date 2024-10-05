using PlatformGame.Character.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlatformGame.Character.Controller
{
    [Serializable]
    public class ActionDataKeyPair
    {
        public string Key;
        public ActionData ActionData;
    }

    public class ActionController : ControllerComponent
    {
        static readonly List<ActionController> mInstances = new();
        public static IEnumerable<ActionController> Instances => mInstances.ToList();

        [FormerlySerializedAs("EditorInputMap")]
        [HideInInspector] public List<ActionDataKeyPair> EditorInputMap;
        [HideInInspector] public Character EditorBeforeCharacter;

        Dictionary<string, ActionData> mInputMap;

        void SetInputAction(List<ActionDataKeyPair> actionKeys)
        {
            mInputMap = new Dictionary<string, ActionData>();
            foreach (var item in actionKeys)
            {
                Debug.Assert(!mInputMap.ContainsKey(item.Key), $"duplicate elements in {name} : {item.Key}");
                mInputMap.Add(item.Key, item.ActionData);
            }
        }

        protected virtual void EnterCommand(ControllerInputData input)
        {
            if (!mInputMap.ContainsKey(input.Key))
            {
                return;
            }

            var actionID = mInputMap[input.Key].ID;
            ControlledCharacter.DoAction(actionID);
        }

        protected override void Awake()
        {
            base.Awake();
            mInputPipeline.InsertPipe(EnterCommand);
            SetInputAction(EditorInputMap);
            mInstances.Add(this);
        }

        void OnDestroy()
        {
            mInstances.Remove(this);
        }

    }
}