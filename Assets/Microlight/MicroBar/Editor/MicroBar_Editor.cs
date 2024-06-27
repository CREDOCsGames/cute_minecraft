using Microlight.MicroEditor;
using UnityEditor;
using UnityEngine;

namespace Microlight.MicroBar {
    // ****************************************************************************************************
    // Custom editor for the MicroBar
    // ****************************************************************************************************
    [CustomEditor(typeof(MicroBar))]
    public class MicroBar_Editor : Editor {
        public override void OnInspectorGUI() {
            serializedObject.Update();

            // Store serialized properties
            SerializedProperty animationsProperty = serializedObject.FindProperty("animations");

            // Header label
            EditorGUILayout.LabelField("Animations", EditorStyles.boldLabel);

            // Animations
            int arraySize = animationsProperty.arraySize;
            for(int i = 0; i < arraySize; i++) {
                SerializedProperty elementProperty = animationsProperty.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(elementProperty, true);

                // Draw the remove button
                GUI.backgroundColor = MicroBar_Theme.RemoveButtonColor;
                if(GUI.Button(RemoveButtonRect(GUILayoutUtility.GetLastRect()), "-")) {
                    RemoveItem(animationsProperty, i);
                    arraySize = animationsProperty.arraySize;
                    i--;
                }
                GUI.backgroundColor = Color.white;
            }

            // Add new animation
            float buttonWidth = EditorGUIUtility.currentViewWidth - 20;
            float buttonX = GUILayoutUtility.GetLastRect().x - 8;
            Rect buttonRect = new Rect(buttonX, GUILayoutUtility.GetLastRect().yMax + MicroEditor_Utility.VerticalSpacing, buttonWidth, MicroEditor_Utility.HeaderLineHeight);

            if(GUI.Button(buttonRect, "Add Animation")) {
                AddNewItemToList(animationsProperty);
            }
            EditorGUILayout.Space(MicroEditor_Utility.HeaderLineHeight + MicroEditor_Utility.VerticalSpacing);

            // To update hovering over elements instantly instead of the delay
            // And it works quite good, very precise
            Rect checkRect = GUILayoutUtility.GetLastRect();
            checkRect.height = checkRect.y + checkRect.height;
            checkRect.y = checkRect.x;
            if(checkRect.Contains(Event.current.mousePosition)) {
                Repaint();
                //EditorUtility.SetDirty(target);
            }

            serializedObject.ApplyModifiedProperties();   // Apply changes
        }

        #region List control
        static void AddNewItemToList(SerializedProperty animationsProperty) {
            // Update the serialized property to reflect the change
            animationsProperty.arraySize++;
            animationsProperty.GetArrayElementAtIndex(animationsProperty.arraySize - 1).isExpanded = true;
            ResetAnimationState(animationsProperty.GetArrayElementAtIndex(animationsProperty.arraySize - 1));
            animationsProperty.serializedObject.ApplyModifiedProperties();
        }
        static void RemoveItem(SerializedProperty animationsProperty, int index) {
            animationsProperty.DeleteArrayElementAtIndex(index);
            animationsProperty.serializedObject.ApplyModifiedProperties();
        }
        static void ResetAnimationState(SerializedProperty animationProperty) {
            animationProperty.FindPropertyRelative("animationType").enumValueIndex = (int)UpdateAnim.Damage;
            animationProperty.FindPropertyRelative("targetImage").objectReferenceValue = null;
            animationProperty.FindPropertyRelative("notBar").boolValue = false;
            
            // Reset commands
            SerializedProperty commandsProperty = animationProperty.FindPropertyRelative("commands");
            commandsProperty.arraySize = 0;
        }
        #endregion

        #region Drawing utilites
        static Rect RemoveButtonRect(Rect position) {
            return new Rect(
                position.xMax - 6 - MicroEditor_Utility.HeaderLineHeight,
                position.y,
                MicroEditor_Utility.HeaderLineHeight,
                MicroEditor_Utility.HeaderLineHeight);
        }
        #endregion
    }
}