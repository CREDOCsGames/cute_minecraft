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
        private Color _color;
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                _renderer.material.color = value;
            }
        }

        private MeshRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
        }
    }

}
