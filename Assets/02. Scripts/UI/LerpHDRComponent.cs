using UnityEngine;

namespace PlatformGame.Util
{
    public class LerpHDRComponent : MonoBehaviour
    {
        MeshRenderer mRenderer;
        public Vector2 Range;
        Color mColor;

        void Awake()
        {
            mRenderer = GetComponent<MeshRenderer>();
            mColor = mRenderer.material.GetColor("_Color");
        }

        public void ChangeHDR(float t, float max)
        {
            var alpha = t / max;
            SetAlpha(alpha);
        }

        public void SetAlpha(float alpha)
        {
            var color = mColor;
            color.a = alpha;

            mRenderer.material.SetColor("_Color", color);
        }
    }

}