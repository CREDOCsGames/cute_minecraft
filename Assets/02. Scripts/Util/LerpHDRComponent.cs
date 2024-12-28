using UnityEngine;

namespace Util
{
    public class LerpHDRComponent : MonoBehaviour
    {
        private MeshRenderer _renderer;
        public Vector2 Range;
        private Color _color;

        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
            _color = _renderer.material.GetColor("_Color");
        }

        public void ChangeHDR(float t, float max)
        {
            var alpha = t / max;
            SetAlpha(alpha);
        }

        private void SetAlpha(float alpha)
        {
            var color = _color;
            color.a = alpha;

            _renderer.material.SetColor("_Color", color);
        }

    }
}