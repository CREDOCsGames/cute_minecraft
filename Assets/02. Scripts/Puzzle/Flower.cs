using Battle;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Puzzle
{
    public class Flower : MonoBehaviour
    {
        [SerializeField] private List<MeshRenderer> _renderers;
        public enum Type : byte
        {
            None, Red, Green
        }

        [field: SerializeField] public HitBoxComponent HitBoxComponent { get; private set; }
        private Color _color;
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                _renderers.ForEach(r => SetColor(r, value));
            }
        }

        private void SetColor(Renderer renderer, Color color)
        {
            if (renderer.material.shader.name == "Toon")
            {
                renderer.material.SetColor("_BaseColor", color);
                // renderer.material.SetColor("Color", color);
            }
            else
            {
                renderer.material.color = color;
            }
        }
    }

}
