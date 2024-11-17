using UnityEngine;
using UnityEngine.Events;

namespace Flow
{
    public class SelectionComponent : MonoBehaviour
    {
        private enum Type
        {
            Title,
            StageSelect
        }

        [SerializeField] private Type _areaType;
        [SerializeField] private UnityEvent _onEnterEvent;
        [SerializeField] private UnityEvent _onSelectEvent;
        [SerializeField] private UnityEvent _onChangeEvent;
        private Selection _selection;

        public void OnEnable()
        {
            _selection.OnEnter();
        }

        public void OnDisable()
        {
            _selection.OnSelect();
        }

        public void OnChange()
        {
            _selection.OnChange();
        }

        private void InvokeOnEnterEvent()
        {
            _onEnterEvent.Invoke();
        }

        private void InvokeOnSelectEvent()
        {
            _onSelectEvent.Invoke();
        }

        private void InvokeOnChangeEvent()
        {
            _onChangeEvent.Invoke();
        }

        private void Awake()
        {
            switch (_areaType)
            {
                case Type.Title:
                    _selection = GameManager.Title;
                    break;
                case Type.StageSelect:
                    _selection = GameManager.StageSelect;
                    break;
                default:
                    Debug.Assert(false, $"{_areaType}");
                    break;
            }

            _selection.OnEnterEvent += InvokeOnEnterEvent;
            _selection.OnSelectEvent += InvokeOnSelectEvent;
            _selection.OnChangeEvent += InvokeOnChangeEvent;
        }

        private void OnDestroy()
        {
            _selection.OnEnterEvent -= InvokeOnEnterEvent;
            _selection.OnSelectEvent -= InvokeOnSelectEvent;
            _selection.OnChangeEvent -= InvokeOnChangeEvent;
        }
    }
}