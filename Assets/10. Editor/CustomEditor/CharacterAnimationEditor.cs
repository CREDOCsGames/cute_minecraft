#if UNITY_EDITOR
using PlatformGame.Character;
using PlatformGame.Character.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace PlatformGame.EditorCustom
{
    [CustomEditor(typeof(CharacterAnimation))]
    public class CharacterAnimationEditor : ReorderableListEditor
    {
        readonly List<string> mTriggers = new List<string>();
        CharacterAnimation mTargetComponent;

        RuntimeAnimatorController mTargetAnimController
        {
            get
            {
                Debug.Assert(mTargetComponent);
                return mTargetComponent.EditorAnimator == null
                    ? null
                    : mTargetComponent.EditorAnimator.runtimeAnimatorController;
            }
        }

        RuntimeAnimatorController mBeforeTargetController
        {
            get => mTargetComponent.BeforeController;
            set => mTargetComponent.BeforeController = value;
        }

        protected override void OnChangedObserveTarget()
        {
            mBeforeTargetController = mTargetAnimController;
            base.OnChangedObserveTarget();
            CreateStateTriggers();
        }

        protected override void ReadObserveTargetData()
        {
            if (mTargetAnimController == null)
            {
                mTriggers.Clear();
                mTargetComponent.EditorStateTriggers.Clear();
            }
            else
            {
                ReadTriggersTo(mTargetComponent.EditorAnimator);
            }
        }

        protected override void CreateReorderableList()
        {
            mList = new ReorderableList(
                serializedObject,
                serializedObject.FindProperty("EditorStateTriggers"),
                true, true, true, true);

            base.CreateReorderableList();
        }

        protected override void DrawFields(Rect rect, int index, bool isActive, bool isFocused)
        {
            if (mTargetComponent.EditorStateTriggers.Count == 0 || mTriggers.Count == 0)
            {
                return;
            }

            rect.y += 2;
            var elementWidth = (rect.width - 30) / 2f;

            EditorGUI.LabelField(new Rect(rect.x, rect.y, elementWidth, EditorGUIUtility.singleLineHeight),
                mTargetComponent.EditorStateTriggers[index].State.ToString());

            var popupIndex = mTriggers.IndexOf(mTargetComponent.EditorStateTriggers[index].Trigger);
            var selectedIndex = EditorGUI.Popup(
                new Rect(rect.x + elementWidth + 10, rect.y, elementWidth, EditorGUIUtility.singleLineHeight),
                popupIndex, mTriggers.ToArray());

            mTargetComponent.EditorStateTriggers[index].Trigger = mTriggers[selectedIndex];
        }

        protected override bool IsChangedObserveTarget()
        {
            return mBeforeTargetController != mTargetAnimController;
        }

        protected override void Awake()
        {
            mTargetComponent = (target as CharacterAnimation);
            mTitle = "(Character State, Trigger)";
            UseAddLock = true;
            UseRemoveLock = true;
        }

        void ReadTriggersTo(Animator animator)
        {
            mTriggers.Clear();

            for (int i = 0; i < animator.parameterCount; i++)
            {
                var parameter = animator.GetParameter(i);
                if (parameter.type != UnityEngine.AnimatorControllerParameterType.Trigger &&
                    mTriggers.Contains(parameter.name))
                {
                    continue;
                }

                mTriggers.Add(parameter.name);
            }
        }

        void CreateStateTriggers()
        {
            mTargetComponent.EditorStateTriggers.Clear();
            if (mTriggers.Count == 0)
            {
                return;
            }

            foreach (CharacterState state in Enum.GetValues(typeof(CharacterState)))
            {
                var item = new StateTriggerPair()
                {
                    State = state,
                    Trigger = mTriggers.First()
                };
                mTargetComponent.EditorStateTriggers.Add(item);
            }
        }
    }
}
#endif