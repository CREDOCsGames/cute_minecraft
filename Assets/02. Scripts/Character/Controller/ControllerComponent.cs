using PlatformGame.Input;
using PlatformGame.Pipeline;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Character.Controller
{
    public class ControllerInputData
    {
        public readonly string Key;
        public readonly ControllerComponent Controller;

        public ControllerInputData(string key, ControllerComponent controller)
        {
            Key = key;
            Controller = controller;
        }
    }

    public class ControllerComponent : MonoBehaviour
    {
        [Header("References")]
        public Character ControlledCharacter;

        [Header("Controls")]
        [SerializeField] bool mIsActive;
        [SerializeField] UnityEvent<ControllerInputData> InputEvents;

        public bool IsActive
        {
            get => mIsActive;
            private set => mIsActive = value;
        }
        protected Pipeline<ControllerInputData> mInputPipeline;

        public virtual void SetActive(bool able)
        {
            IsActive = able;
        }

        protected virtual void Awake()
        {
            mInputPipeline = Pipelines.Instance.PlayerCharacterControllerPipeline;
            mInputPipeline.InsertPipe((x) => InputEvents.Invoke(x));
        }

        void Update()
        {
            if (!IsActive)
            {
                return;
            }

            var inputMap = ActionKey.GetKeyDownMap();
            foreach (var input in inputMap)
            {
                if (!input.Value)
                {
                    continue;
                }

                var inputData = new ControllerInputData(input.Key, this);
                mInputPipeline.Invoke(inputData);
            }
        }
    }

}
