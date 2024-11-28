using UnityEngine;
using UnityEditor;
using System;
using Util;
using Cinemachine.Editor;

public class MatrixBoolLayout
{
    MatrixBool mMatrixBool;
    int mRowCount;
    int mColumnCount;

    public MatrixBoolLayout()
    {
        mMatrixBool = new MatrixBool();
        mRowCount = mMatrixBool.Matrix.Count;
        mColumnCount = mRowCount > 0 ? mMatrixBool.Matrix[0].List.Count : 0;
    }

    public void DrawMatrix()
    {
        if (GUILayout.Button("Add Row"))
        {
            AddRow();
        }

        else if (GUILayout.Button("Remove Row"))
        {
            RemoveRow();
        }

        if (GUILayout.Button("Add Column"))
        {
            AddColumn();

        }
        else if (GUILayout.Button("Remove Column"))
        {
            RemoveColumn();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(mMatrixBool);
        }

        DrawGUI();
    }

    void DrawGUI()
    {
        EditorGUILayout.LabelField("Matrix Values", EditorStyles.boldLabel);

        for (int i = 0; i < mRowCount; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < mColumnCount; j++)
            {

                mMatrixBool.Matrix[i].List[j] = EditorGUILayout.Toggle(mMatrixBool.Matrix[i].List[j], GUILayout.Width(20), GUILayout.Height(20));
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    void AddRow()
    {
        mRowCount++;
        mMatrixBool.Matrix.Add(new CustomList<bool>(mColumnCount, false));
        if (mColumnCount == 0)
        {
            AddColumn();
        }
    }

    void RemoveRow()
    {
        if (mRowCount == 0)
        {
            return;
        }
        if (--mRowCount == 0)
        {
            ClearMatrix();
            return;
        }
        mMatrixBool.Matrix.RemoveAt(mRowCount);
    }

    void AddColumn()
    {
        mColumnCount++;
        foreach (var row in mMatrixBool.Matrix)
        {
            while (row.List.Count < mColumnCount)
            {
                row.List.Add(false);
            }
        }
        if (mRowCount == 0)
        {
            AddRow();
        }
    }

    void RemoveColumn()
    {
        if (mColumnCount == 0)
        {
            return;
        }
        if (--mColumnCount == 0)
        {
            ClearMatrix();
            return;
        }
        foreach (var row in mMatrixBool.Matrix)
        {
            row.List.RemoveAt(mColumnCount);
        }
    }

    void ClearMatrix()
    {
        mMatrixBool.Matrix.Clear();
        mRowCount = 0;
        mColumnCount = 0;
    }

}

public class HexahedronButton
{
    public float ButtonSize = 100f;
    public event Action OnClickTop;
    public event Action OnClickBottom;
    public event Action OnClickLeft;
    public event Action OnClickRight;
    public event Action OnClickFront;
    public event Action OnClickBack;
    private readonly Texture[] _buttonTextures;

    public HexahedronButton(Texture[] buttonTextures)
    {
        _buttonTextures = buttonTextures;
    }

    public void DrawButtons()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(ButtonSize + 3f);
        if (GUILayout.Button(_buttonTextures[0], new GUILayoutOption[] { GUILayout.Width(ButtonSize), GUILayout.Height(ButtonSize) }))
        {
            OnClickTop();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(_buttonTextures[1], new GUILayoutOption[] { GUILayout.Width(ButtonSize), GUILayout.Height(ButtonSize) }))
        {
            OnClickLeft();
        }
        if (GUILayout.Button(_buttonTextures[2], new GUILayoutOption[] { GUILayout.Width(ButtonSize), GUILayout.Height(ButtonSize) }))
        {
            OnClickFront();
        }
        if (GUILayout.Button(_buttonTextures[3], new GUILayoutOption[] { GUILayout.Width(ButtonSize), GUILayout.Height(ButtonSize) }))
        {
            OnClickRight();
        }
        if (GUILayout.Button(_buttonTextures[4], new GUILayoutOption[] { GUILayout.Width(ButtonSize), GUILayout.Height(ButtonSize) }))
        {
            OnClickBack();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Space(ButtonSize + 3f);
        if (GUILayout.Button(_buttonTextures[5], new GUILayoutOption[] { GUILayout.Width(ButtonSize), GUILayout.Height(ButtonSize) }))
        {
            OnClickBottom();
        }
        GUILayout.EndHorizontal();
    }
}

public static class GUIHelper
{
    public static GUIStyle GetTitleStyle()
    {
        var titleStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 32,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.UpperLeft,
            normal = { textColor = Color.gray }
        };
        return titleStyle;
    }
}

[CustomEditor(typeof(Test))]
public class ImageClickEditor : Editor
{
    private HexahedronButton _hexahedronButton;
    private MatrixBoolLayout _matrixBool;
    private byte _foreFace;
    public byte ForeFace
    {
        get => 0;
        private set
        {
            _foreFace = value;
        }
    }
    private readonly string[] _faces =
    {
        "OnTopFaceToFore",
        "OnLeftFaceToFore",
        "OnFrontFaceToFore",
        "OnRightFaceToFore",
        "OnBackFaceToFore",
        "OnBottomFaceToFore"
    };

    private void OnEnable()
    {
        ForeFace = 0;
        _hexahedronButton = new HexahedronButton(new[] {
            (Texture)AssetDatabase.LoadAssetAtPath("Assets/10. Editor/Image/dice 1.JPG", typeof(Texture)),
            (Texture)AssetDatabase.LoadAssetAtPath("Assets/10. Editor/Image/dice 2.JPG", typeof(Texture)),
            (Texture)AssetDatabase.LoadAssetAtPath("Assets/10. Editor/Image/dice 3.JPG", typeof(Texture)),
            (Texture)AssetDatabase.LoadAssetAtPath("Assets/10. Editor/Image/dice 4.JPG", typeof(Texture)),
            (Texture)AssetDatabase.LoadAssetAtPath("Assets/10. Editor/Image/dice 5.JPG", typeof(Texture)),
            (Texture)AssetDatabase.LoadAssetAtPath("Assets/10. Editor/Image/dice 6.JPG", typeof(Texture)),
        });
        _hexahedronButton.ButtonSize = 80f;
        SetEvent();

        _matrixBool = new MatrixBoolLayout();
    }

    bool toggle;
    public override void OnInspectorGUI()
    {
        GUILayout.Label("[Options]", GUIHelper.GetTitleStyle());
        toggle = GUILayout.Toggle(toggle, "Use Link");

        GUILayout.Label("[Select Face]", GUIHelper.GetTitleStyle());
        _hexahedronButton.DrawButtons();

        GUILayout.Label("[Map Set]", GUIHelper.GetTitleStyle());
        _matrixBool.DrawMatrix();

        GUILayout.Label("[Event Set]", GUIHelper.GetTitleStyle());

        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty(_faces[_foreFace]), true);
        serializedObject.ApplyModifiedProperties();
    }

    private void SetEvent()
    {
        _hexahedronButton.OnClickTop += () => ForeFace = 0;
        _hexahedronButton.OnClickLeft += () => ForeFace = 1;
        _hexahedronButton.OnClickFront += () => ForeFace = 2;
        _hexahedronButton.OnClickRight += () => ForeFace = 3;
        _hexahedronButton.OnClickBack += () => ForeFace = 4;
        _hexahedronButton.OnClickBottom += () => ForeFace = 5;
    }

}
