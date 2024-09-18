using PlatformGame.Manager;
using UnityEngine;
using UnityEngine.Events;

public class SelectionComponent : MonoBehaviour
{
    enum Type { Title, StageSelect }
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
