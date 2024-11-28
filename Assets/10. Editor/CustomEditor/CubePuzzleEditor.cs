#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Puzzle;
using UnityEditor;
using UnityEngine;
using Util;

namespace CuzzleEditor
{
    public class PuzzleMap
    {
        public readonly Matrix<byte> _map;
        private readonly int _buttonSize;
        private readonly List<Texture> _icons = AssetDatabase.LoadAssetAtPath<ImageList>("Assets/10. Editor/Puzzle Brush.asset").Images;
        private Vector2 _scrollPosition;
        private byte _columnCount = 3;
        private byte _brushIndex = 0;

        public PuzzleMap(Matrix<byte> map, int buttonSize)
        {
            _map = map;
            _buttonSize = buttonSize;
        }
        public void DrawBrushButton()
        {
            GUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(_columnCount * (_buttonSize + 4)), GUILayout.Height(_columnCount * (_buttonSize + 4)));
            for (byte i = 0; i < _icons.Count; i++)
            {
                if (i % _columnCount == 0)
                {
                    GUILayout.BeginHorizontal();
                }
                if (GUILayout.Button(_icons[i], GUILayout.Width(_buttonSize), GUILayout.Height(_buttonSize)))
                {
                    _brushIndex = i;
                    break;
                }
                if (i % _columnCount == _columnCount - 1)
                {
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndScrollView();
        }
        public void DrawRankButton(out bool changed)
        {
            changed = false;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Rank"))
            {
                _map.AddRank();
                changed = true;
            }
            else if (GUILayout.Button("Subtract Rank"))
            {
                _map.SubtractRank();
                changed = true;
            }
            GUILayout.EndHorizontal();

        }
        public void DrawToggleButton()
        {

            for (int i = 0; i < _map.ElementsCount; i++)
            {
                if (i % _map.ColumnsCount == 0)
                {
                    GUILayout.BeginHorizontal();
                }
                DrawToggle(i);

                if (i % _map.ColumnsCount == _map.ColumnsCount - 1)
                {
                    GUILayout.EndHorizontal();
                }
            }
        }
        private void DrawToggle(int i)
        {
            var click = false;

            if (_map.TryGetElement(i / _map.ColumnsCount, i % _map.ColumnsCount, out var toggle))
            {
                click |= GUILayout.Button(GetIcon(toggle), GUILayout.Width(_buttonSize), GUILayout.Height(_buttonSize));
            }
            else
            {
                click |= GUILayout.Button("-1", GUILayout.Width(_buttonSize), GUILayout.Height(_buttonSize));
            }


            if (click && _map.TryGetElement(i / _map.ColumnsCount, i % _map.ColumnsCount, out var value))
            {
                _map.SetElement(i / _map.ColumnsCount, i % _map.ColumnsCount, _brushIndex);
            }
        }
        private Texture GetIcon(int i)
        {
            Texture icon;
            if (_icons != null && i < _icons.Count)
            {
                icon = _icons[i];
            }
            else
            {
                icon = new Texture2D(0, 0);
            }
            return icon;
        }
    }


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
        private PuzzleMap _puzzleMap;
        private Face _selectedFace;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _cubeButton.DrawButtons();
            GUILayout.TextArea(_selectedFace.ToString());
            _puzzleMap?.DrawBrushButton();
            bool changed = false;
            _puzzleMap?.DrawRankButton(out changed);
            _puzzleMap?.DrawToggleButton();
            if (changed)
            {
                (target as CubePuzzleComponent).MapData.ColumnCount = (target as CubePuzzleComponent).MapData.Matrix.ColumnsCount;
            }
        }

        private void Awake()
        {
            if (target is CubePuzzleComponent cubePuzzle && cubePuzzle.MapData == null)
            {
                Scriptable_MatrixByte obj = ScriptableObject.CreateInstance<Scriptable_MatrixByte>();
                cubePuzzle.MapData = obj;
                AssetDatabase.CreateAsset(obj, $"Assets/10. Editor/Cookie/{obj.GetHashCode()}.asset");
                AssetDatabase.SaveAssets();
            }
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

            _puzzleMap = new((target as CubePuzzleComponent).MapData.Matrix, 25);

        }

        private void OnDisable()
        {
            _cubeButton = null;
        }
    }


}
#endif