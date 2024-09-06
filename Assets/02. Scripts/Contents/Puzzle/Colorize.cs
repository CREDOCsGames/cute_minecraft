using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Util
{
    [CreateAssetMenu(menuName ="Custom/Util/Colorize")]
    public class Colorize : UniqueScriptableObject<Colorize>
    {
        public void Invoke(List<MeshRenderer> materials, Color color)
        {
            foreach (var mat in materials)
            {
                mat.material.color = color;
            }
        }

    }
}
