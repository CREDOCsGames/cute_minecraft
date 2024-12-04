#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Util;

namespace CuzzleEditor
{
    public class PuzzleMap
    {
        public Matrix<byte> _map => Map.Matrix;
        [SerializeField] public Scriptable_MatrixByte Map;
        private readonly int _buttonSize;
        private readonly List<Texture> _icons = AssetDatabase.LoadAssetAtPath<ImageList>("Assets/10. Editor/Puzzle Brush.asset").Images;
        private Vector2 _scrollPosition;
        private byte _columnCount = 3;
        private byte _brushIndex = 0;

        public PuzzleMap(Scriptable_MatrixByte map, int buttonSize)
        {
            Map = map;
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
        public void DrawToggleButton(out bool changed)
        {
            bool change = false;
            for (int i = 0; i < _map.ElementsCount; i++)
            {
                if (i % _map.ColumnsCount == 0)
                {
                    GUILayout.BeginHorizontal();
                }
                DrawToggle(i, out var c);
                change |= c;
                if (i % _map.ColumnsCount == _map.ColumnsCount - 1)
                {
                    GUILayout.EndHorizontal();
                }
            }
            changed = change;
        }
        private void DrawToggle(int i, out bool changed)
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
            changed = click;
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


}
#endif