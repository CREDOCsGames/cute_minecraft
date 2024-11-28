using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    [CreateAssetMenu(menuName = "Custom/Data/ImageList")]
    public class ImageList : ScriptableObject
    {
        public List<Texture> Images;
    }
}