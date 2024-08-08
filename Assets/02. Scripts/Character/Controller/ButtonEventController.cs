using PlatformGame.Character.Combat;
using Unity.VisualScripting;
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

        public void ClearEvent()
        {
            mButtonAction = null;
            mBlockEvent.RemoveAllListeners();
        }

        protected override void EnterCommand(ControllerInputData input)
        {
            base.EnterCommand(input);

            var map = GetAxisRawMap();
            //if (!map[KEY_ATTACK])
            if(!UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
            {
                return;
            }

            if (mButtonAction != null)
            {
                var actionID = mButtonAction.ID;
                ControlledCharacter.DoAction(actionID);
                mButtonAction = null;
            }

            mBlockEvent?.Invoke();
            mBlockEvent = null;
            mButtonAnim.gameObject.SetActive(false);
        }

        void OnTriggerEnter(Collider other)
        {
            var block = other.GetComponent<BlockEvent>();
            if (block == null || !block.IsEnable)
            {
                return;
            }
            SetButtonAction(block.ButtonAction);
            mButtonAnim.transform.SetParent(block.transform);
            mButtonAnim.transform.localPosition = Vector3.right + Vector3.up + Vector3.back;
            mButtonAnim.gameObject.SetActive(true);
            mButtonAnim.Play("Push");
            mBlockEvent = block.ButtonEvent;
        }

    }
}
