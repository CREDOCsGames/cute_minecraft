#if UNITY_EDITOR
using System;
using Puzzle;
using UnityEditor;
using UnityEngine;
using Util;

namespace CuzzleEditor
{

    [CustomEditor(typeof(CubePuzzleComponent))]
    public class CubePuzzleEditor : Editor
    {
        private CubeButton _cubeButton;
        private PuzzleMapDrawer _puzzleMap;
        private Face _selectedFace;
        private int _selectedFaceIndex => (int)_selectedFace;

        public override void OnInspectorGUI()
        {

            base.OnInspectorGUI();
            if (target is not CubePuzzleComponent cubePuzzle)
            {
                return;
            }

            _cubeButton.DrawButtons();
            GUILayout.TextArea(_selectedFace.ToString());
            _puzzleMap?.DrawBrushButton();
            bool changed = false;
            _puzzleMap?.DrawRankButton(out changed);
            bool changed2 = false;
            _puzzleMap?.DrawToggleButton(out changed2);
            if (changed || changed2)
            {
                (target as CubePuzzleComponent).MapData[_selectedFaceIndex].ColumnCount = (target as CubePuzzleComponent).MapData[_selectedFaceIndex].Matrix.ColumnsCount;
                _puzzleMap.Map.Save();
            }
        }


        private void OnEnable()
        {
            if (target is not CubePuzzleComponent cubePuzzle)
            {
                return;
            }

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

            _cubeButton.OnTopButtonClick += () => _puzzleMap = new(cubePuzzle.MapData[_selectedFaceIndex], 25);
            _cubeButton.OnLeftButtonClick += () => _puzzleMap = new(cubePuzzle.MapData[_selectedFaceIndex], 25);
            _cubeButton.OnFrontButtonClick += () => _puzzleMap = new(cubePuzzle.MapData[_selectedFaceIndex], 25);
            _cubeButton.OnRightButtonClick += () => _puzzleMap = new(cubePuzzle.MapData[_selectedFaceIndex], 25);
            _cubeButton.OnBackButtonClick += () => _puzzleMap = new(cubePuzzle.MapData[_selectedFaceIndex], 25);
            _cubeButton.OnBottomButtonClick += () => _puzzleMap = new(cubePuzzle.MapData[_selectedFaceIndex], 25);

            _puzzleMap = new(cubePuzzle.MapData[_selectedFaceIndex], 25);

        }

        private void OnDisable()
        {
            _cubeButton = null;
        }
    }


}
#endif