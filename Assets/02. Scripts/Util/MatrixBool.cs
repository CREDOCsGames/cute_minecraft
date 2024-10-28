using UnityEngine;

namespace Util
{
    [CreateAssetMenu(menuName = "Custom/Data/MatrixBool")]
    public class MatrixBool
        : ScriptableMatrix<bool>
    {
        public int Column => Matrix.Count == 0 ? 0 : Matrix[0].List.Count;
        public int Row => Matrix.Count;
    }
}