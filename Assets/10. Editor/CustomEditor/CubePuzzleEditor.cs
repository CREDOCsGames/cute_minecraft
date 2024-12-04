#if UNITY_EDITOR
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
            bool changed2 = false;
            _puzzleMap?.DrawToggleButton(out changed2);
            if (changed || changed2)
            {
                (target as CubePuzzleComponent).MapData.ColumnCount = (target as CubePuzzleComponent).MapData.Matrix.ColumnsCount;
                _puzzleMap.Map.Save();
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

            _puzzleMap = new((target as CubePuzzleComponent).MapData, 25);

        }

        private void OnDisable()
        {
            _cubeButton = null;
        }
    }


}
#endif