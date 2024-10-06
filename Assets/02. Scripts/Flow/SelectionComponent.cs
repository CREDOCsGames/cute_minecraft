using UnityEngine;
using UnityEngine.Events;

namespace Flow
{
    public class SelectionComponent : MonoBehaviour
    {
        enum Type
        {
            Title,
            StageSelect
        }

        [SerializeField] Type AreaType;
        [SerializeField] UnityEvent OnEnterEvent;
        [SerializeField] UnityEvent OnSelectEvent;
        [SerializeField] UnityEvent OnChangeEvent;
        Selection mSelection;

        public void OnEnable()
        {
            mSelection.OnEnter();
        }

        public void OnDisable()
        {
            mSelection.OnSelect();
        }

        public void OnChange()
        {
            mSelection.OnChange();
        }

        void InvokeOnEnterEvent()
        {
            OnEnterEvent.Invoke();
        }

        void InvokeOnSelectEvent()
        {
            OnSelectEvent.Invoke();
        }

        void InvokeOnChangeEvent()
        {
            OnChangeEvent.Invoke();
        }

        void Awake()
        {
            switch (AreaType)
            {
                case Type.Title:
                    mSelection = GameManager.Title;
                    break;
                case Type.StageSelect:
                    mSelection = GameManager.StageSelect;
                    break;
                default:
                    Debug.Assert(false, $"{AreaType}");
                    break;
            }

            mSelection.OnEnterEvent += InvokeOnEnterEvent;
            mSelection.OnSelectEvent += InvokeOnSelectEvent;
            mSelection.OnChangeEvent += InvokeOnChangeEvent;
        }

        void OnDestroy()
        {
            mSelection.OnEnterEvent -= InvokeOnEnterEvent;
            mSelection.OnSelectEvent -= InvokeOnSelectEvent;
            mSelection.OnChangeEvent -= InvokeOnChangeEvent;
        }
    }
}