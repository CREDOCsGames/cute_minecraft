#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public abstract class ReorderableListEditor : Editor
{
    protected ReorderableList mList;
    protected string mTitle = "ReorderableList";
    bool mUseAddLock;

    protected bool UseAddLock
    {
        get => mUseAddLock;

        set
        {
            mUseAddLock = value;
            if (mUseAddLock && mList != null)
            {
                mList.onAddCallback = LockFuntion;
            }
        }
    }

    bool mUseRemoveLock;

    protected bool UseRemoveLock
    {
        get => mUseRemoveLock;
        set
        {
            mUseAddLock = value;
            if (mUseRemoveLock && mList != null)
            {
                mList.onRemoveCallback = LockFuntion;
            }
        }
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        UpdateInspectorGUI();

        if (IsChangedObserveTarget())
        {
            OnChangedObserveTarget();
        }

        if (EditorGUI.EndChangeCheck())
        {
            UpdateTargetData();
        }
    }

    protected virtual void OnChangedObserveTarget()
    {
        ReadObserveTargetData();
    }

    protected virtual void CreateReorderableList()
    {
        Debug.Assert(mList != null);
        mList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect,
                mTitle);
        };
        mList.drawElementCallback = DrawFields;

        if (UseAddLock)
        {
            mList.onAddCallback = LockFuntion;
        }

        if (UseRemoveLock)
        {
            mList.onRemoveCallback = LockFuntion;
        }
    }

    protected virtual void Awake()
    {
    }

    protected abstract bool IsChangedObserveTarget();
    protected abstract void ReadObserveTargetData();
    protected abstract void DrawFields(Rect rect, int index, bool isActive, bool isFocused);

    protected virtual void OnEnable()
    {
        CreateReorderableList();
        if (IsChangedObserveTarget())
        {
            OnChangedObserveTarget();
        }
        else
        {
            ReadObserveTargetData();
        }
    }

    void UpdateInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.LabelField("");
        mList.DoLayoutList();
    }

    void UpdateTargetData()
    {
        EditorUtility.SetDirty(target);
        serializedObject.ApplyModifiedProperties();
    }

    static void LockFuntion(ReorderableList list)
    {
    }
}
#endif