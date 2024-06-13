#if UNITY_EDITOR
using PlatformGame.Character;
using PlatformGame.Character.Controller;
using PlatformGame.Input;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(ActionController),true)]
public class ActionControllerEditor : ReorderableListEditor
{
    ActionController mController;
    Character mControlledCharacter => mController.ControlledCharacter;

    readonly List<string> mAbilities = new List<string>();
    List<string> mInputs;

    protected override void CreateReorderableList()
    {
        mList = new ReorderableList(
            serializedObject,
            serializedObject.FindProperty("EditorInputMap"),
            true, true, true, true);
        Debug.Assert(mList != null, "EditorInputMap Is No Exits.");
        base.CreateReorderableList();
    }

    protected override void DrawFields(Rect rect, int index, bool isActive, bool isFocused)
    {
        if (mInputs.Count == 0 ||
            mAbilities.Count == 0 ||
            mControlledCharacter == null)
        {
            return;
        }

        rect.y += 2;
        var elementWidth = (rect.width - 30) / 2f;

        DrawInputKeyField(rect, index, elementWidth);
        DrawActionField(rect, index, elementWidth);
    }

    protected override void OnChangedObserveTarget()
    {
        mController.EditorBeforeCharacter = mControlledCharacter;
        ControlButtonLock();

        mController.EditorInputMap.Clear();
        base.OnChangedObserveTarget();
    }

    protected override bool IsChangedObserveTarget()
    {
        return mController.EditorBeforeCharacter != mControlledCharacter;
    }

    protected override void ReadObserveTargetData()
    {
        mAbilities.Clear();
        if (mControlledCharacter?.HasAbilities == null)
        {
            return;
        }

        foreach (var id in mControlledCharacter.HasAbilities.Library.Keys)
        {
            mAbilities.Add(id.ToString());
        }
    }

    protected override void Awake()
    {
        base.Awake();
        mTitle = "(InputKey, Action)";
        mController = (ActionController)target;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        mInputs = ActionKey.InputKeys;
        ControlButtonLock();
    }

    void DrawActionField(Rect rect, int index, float elementWidth)
    {
        var actionIndex = FindActionIndex(index);
        var selectedAction = EditorGUI.Popup(
            new Rect(rect.x + elementWidth + 10, rect.y, elementWidth, EditorGUIUtility.singleLineHeight),
            actionIndex, mAbilities.ToArray());
        mController.EditorInputMap[index].ActionData = mControlledCharacter.HasAbilities.Library.First().Value;
        if (actionIndex == -1)
        {
            return;
        }

        var id = uint.Parse(mAbilities[selectedAction]);
        mController.EditorInputMap[index].ActionData = mControlledCharacter.HasAbilities.Library[id];
    }

    void DrawInputKeyField(Rect rect, int index, float elementWidth)
    {
        var keyIndex = FindInputKeyIndex(index);
        var selectedKey = EditorGUI.Popup(new Rect(rect.x, rect.y, elementWidth, EditorGUIUtility.singleLineHeight),
            keyIndex, mInputs.ToArray());
        mController.EditorInputMap[index].Key = selectedKey == -1 ? mInputs[0] : mInputs[selectedKey];
    }

    int FindInputKeyIndex(int index)
    {
        var result = mInputs.IndexOf(mController.EditorInputMap[index].Key);
        return result;
    }

    int FindActionIndex(int index)
    {
        var id = mController.EditorInputMap[index].ActionData?.ID ?? 0;
        return mAbilities.FindIndex(x => x.Equals(id.ToString()));
    }

    void ControlButtonLock()
    {
        var isLock = mControlledCharacter == null ||
                     mControlledCharacter.HasAbilities == null ||
                     mControlledCharacter.HasAbilities.Library.Count == 0;
        UseAddLock = isLock;
        UseRemoveLock = isLock;
    }
}
#endif