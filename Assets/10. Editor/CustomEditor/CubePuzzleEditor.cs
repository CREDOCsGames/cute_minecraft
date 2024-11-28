#if UNITY_EDITOR
using System;
using Puzzle;
using UnityEditor;
using UnityEngine;

namespace CuzzleEditor
{

    internal class CubeButton
    {
        internal event Action OnTopButtonClick;
        internal event Action OnLeftButtonClick;
        internal event Action OnFrontButtonClick;
        internal event Action OnRightButtonClick;
        internal event Action OnBackButtonClick;
        internal event Action OnBottomButtonClick;
        private readonly float _length = 75;
        private readonly Texture[] _buttonImages;

        internal CubeButton(float length, Texture[] buttonImages)
        {
            _length = length;
            _buttonImages = buttonImages;
        }

        public void DrawButtons()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(_length + 3);
            if (GUILayout.Button(_buttonImages[(int)Face.top], GUILayout.Width(_length), GUILayout.Height(_length)))
            {
                OnTopButtonClick?.Invoke();
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(_buttonImages[(int)Face.left], GUILayout.Width(_length), GUILayout.Height(_length)))
            {
                OnLeftButtonClick?.Invoke();
            }
            else if (GUILayout.Button(_buttonImages[(int)Face.front], GUILayout.Width(_length), GUILayout.Height(_length)))
            {
                OnFrontButtonClick?.Invoke();
            }
            else if (GUILayout.Button(_buttonImages[(int)Face.right], GUILayout.Width(_length), GUILayout.Height(_length)))
            {
                OnRightButtonClick?.Invoke();
            }
            else if (GUILayout.Button(_buttonImages[(int)Face.back], GUILayout.Width(_length), GUILayout.Height(_length)))
            {
                OnBackButtonClick?.Invoke();
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Space(_length + 3);
            if (GUILayout.Button(_buttonImages[(int)Face.bottom], GUILayout.Width(_length), GUILayout.Height(_length)))
            {
                OnBottomButtonClick?.Invoke();
            }
            GUILayout.EndHorizontal();
        }
    }

    [CustomEditor(typeof(CubePuzzleComponent))]
    public class CubePuzzleEditor : Editor
    {
        private CubeButton _cubeButton;
        private Face _selectedFace;

        public override void OnInspectorGUI()
        {
            _cubeButton.DrawButtons();
            GUILayout.TextArea(_selectedFace.ToString());
        }

        private void OnEnable()
        {
            _cubeButton = new CubeButton(75f, new[]
            {
                AssetDatabase.LoadAssetAtPath<Texture>("Assets/10. Editor/Image/1.png"),
                AssetDatabase.LoadAssetAtPath<Texture>("Assets/10. Editor/Image/2.png"),
                AssetDatabase.LoadAssetAtPath<Texture>("Assets/10. Editor/Image/3.png"),
                AssetDatabase.LoadAssetAtPath<Texture>("Assets/10. Editor/Image/4.png"),
                AssetDatabase.LoadAssetAtPath<Texture>("Assets/10. Editor/Image/5.png"),
                AssetDatabase.LoadAssetAtPath<Texture>("Assets/10. Editor/Image/6.png"),
            });

            _cubeButton.OnTopButtonClick += () => _selectedFace = Face.top;
            _cubeButton.OnLeftButtonClick += () => _selectedFace = Face.left;
            _cubeButton.OnFrontButtonClick += () => _selectedFace = Face.front;
            _cubeButton.OnRightButtonClick += () => _selectedFace = Face.right;
            _cubeButton.OnBackButtonClick += () => _selectedFace = Face.back;
            _cubeButton.OnBottomButtonClick += () => _selectedFace = Face.bottom;
        }

        private void OnDisable()
        {
            _cubeButton = null;
        }
    }


}
#endif