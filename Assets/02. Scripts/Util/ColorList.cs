using UnityEngine;

namespace Util
{
    [CreateAssetMenu(menuName = "Data/ColorList", fileName = "ColorList")]
    public class ColorList : ScriptableList<Color>
    {
        public static readonly ColorList DEFAULT;
    }

}
