#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;
using Util;

[CustomEditor(typeof(MatrixBool))]
public class MatrixBoolEditor : Editor
{
    MatrixBool mMatrixBool;
    int mRowCount;
    int mColumnCount;

    void OnEnable()
    {
        mMatrixBool = (MatrixBool)target;
        mRowCount = mMatrixBool.Matrix.Count;
        mColumnCount = mRowCount > 0 ? mMatrixBool.Matrix[0].List.Count() : 0;
    }

    public override void OnInspectorGUI()
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
#endif