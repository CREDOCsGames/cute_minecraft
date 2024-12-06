using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Util
{
    [CreateAssetMenu(menuName = "Custom/Data/MatrixByte")]
    [Serializable]
    public class Scriptable_MatrixByte : ScriptableObject
    {
        private Matrix<byte> _matrix;
        public Matrix<byte> Matrix
        {
            get
            {
                if (_matrix == null)
                {
                    _matrix = new Matrix<byte>(0);
                    if (data != null)
                    {
                        _matrix.SetMatrix(data, ColumnCount);
                    }
                }

                if (_matrix.Matrixt == null)
                {
                    _matrix.SetMatrix(data, ColumnCount);
                }
                return _matrix;
            }
        }
        public List<byte> data = new();
        public int ColumnCount;

        public void Save()
        {
            data.Clear();
            foreach (var item in Matrix.GetElements())
            {
                data.Add(item);
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
}