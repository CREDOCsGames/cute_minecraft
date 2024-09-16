using PlatformGame.Manager;
using UnityEngine;
using UnityEngine.Events;

public class SelectionEventHandler : MonoBehaviour
{
    enum Type { Title, StageSelect }
    [SerializeField] Type AreaType;
    [SerializeField] UnityEvent OnEnterEvent;
    [SerializeField] UnityEvent OnSelectEvent;
    [SerializeField] UnityEvent OnChangeEvent;
    Selection mSelection;

    public void OnEnter()
    {
        mSelection.OnEnter();
    }

    public void OnSelection()
    {
        mSelection.OnSelect();
    }

    public void OnCange()
    {
        mSelection.OnChange();
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
        mSelection.OnEnterEvent += () => OnEnterEvent.Invoke();
        mSelection.OnSelectEvent += () => OnSelectEvent.Invoke();
        mSelection.OnChangeEvent += () => OnChangeEvent.Invoke();
    }

    void OnDestroy()
    {
        mSelection.OnEnterEvent -= () => OnEnterEvent.Invoke();
        mSelection.OnSelectEvent -= () => OnSelectEvent.Invoke();
        mSelection.OnChangeEvent -= () => OnChangeEvent.Invoke();
    }

}
