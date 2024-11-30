using System;
using System.Collections.Generic;
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
                return _matrix;
            }
        }
        public List<byte> data;
        public int ColumnCount;
    }
}