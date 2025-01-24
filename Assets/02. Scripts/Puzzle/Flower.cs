using Battle;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    public class Flower : MonoBehaviour
    {
        [SerializeField] private List<MeshRenderer> _renderers;
        public enum Type : byte
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
                _renderers.ForEach(r => r.material.color = value);
            }
        }
    }

}
