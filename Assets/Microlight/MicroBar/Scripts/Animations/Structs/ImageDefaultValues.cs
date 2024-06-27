using UnityEngine;
using UnityEngine.UI;

namespace Microlight.MicroBar {
    // ****************************************************************************************************
    // Stores default values for an image
    // ****************************************************************************************************
    internal readonly struct ImageDefaultValues {
        readonly Color color;
        readonly float fade;
        readonly float fill;
        readonly Vector2 position;
        readonly float rotation;
        readonly Vector2 scale;
        readonly Vector2 anchorPosition;

        internal readonly Color Color => color;
        internal readonly float Fade => fade;
        internal readonly float Fill => fill;
        internal readonly Vector2 Position => position;
        internal readonly float Rotation => rotation;
        internal readonly Vector2 Scale => scale;
        internal readonly Vector2 AnchorPosition => anchorPosition;

        internal ImageDefaultValues(Image image) {
            color = image.color;
            fade = image.color.a;
            fill = image.fillAmount;
            position = image.rectTransform.localPosition;
            rotation = image.rectTransform.localRotation.eulerAngles.z;
            scale = image.rectTransform.localScale;
            anchorPosition = image.rectTransform.anchoredPosition;
        }
    }
}