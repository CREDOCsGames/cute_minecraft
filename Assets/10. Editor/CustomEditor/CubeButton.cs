#if UNITY_EDITOR
using Puzzle;
using System;
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


}
#endif