using Battle;
using UnityEngine;

namespace Puzzle
{
    public class Flower : MonoBehaviour
    {
        public enum FlowerType : byte
        {
            None, Red, Green
        }

        public HitBoxComponent HitBoxComponent;
        Color mColor;
        public Color Color
        {
            get => mColor;
            set
            {
                mColor = value;
                mRenderer.material.color = value;
            }
        }

        MeshRenderer mRenderer;

        private void Awake()
        {
            mRenderer = GetComponent<MeshRenderer>();
        }
    }

}
