using PlatformGame.Character.Combat;
using UnityEngine;
using UnityEngine.Events;
using static PlatformGame.Input.ActionKey;

namespace PlatformGame.Character.Controller
{
    public class ButtonEventController : ActionController
    {
        ActionData mButtonAction;
        UnityEvent mBlockEvent;
        [SerializeField] Animator mButtonAnim;
        public void SetButtonAction(ActionData actionData)
        {
            mButtonAction = actionData;
        }

        protected override void EnterCommand(ControllerInputData input)
        {
            base.EnterCommand(input);

            var map = GetKeyDownMap();
            if (!map[KEY_ATTACK])
            {
                return;
            }
            mBlockEvent.Invoke();

            if (mButtonAction == null)
            {
                return;
            }
            var actionID = mButtonAction.ID;
            ControlledCharacter.DoAction(actionID);
        }

        void OnTriggerEnter(Collider other)
        {
            var block = other.GetComponent<BlockEvent>();
            if (block == null)
            {
                return;
            }
            SetButtonAction(block.ButtonAction);
            mButtonAnim.transform.SetParent(block.transform);
            mButtonAnim.transform.localPosition = Vector3.zero + Vector3.up;
            mButtonAnim.Play("Push");
            mBlockEvent = block.ButtonEvent;
        }

    }
}
