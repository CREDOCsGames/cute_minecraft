using Battle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    public class Flower : MonoBehaviour
    {
        public static readonly Flower DEFAULT;
        public enum Type : byte
        {
            None, Red, Green
        }
        public event Action<HitBoxCollision> OnHit;
        public byte[] Index { get; set; }
        private Color _color;
        public Color Color
        {
            get => _color;
            set
            {
                gameObject.SetActive(value != Color.clear);
                _color = value;
                _renderers.ForEach(r => SetColor(r, value));
            }
        }
        [field: SerializeField] public HitBoxComponent HitBoxComponent { get; private set; }
        [SerializeField] private List<MeshRenderer> _renderers;

        private void Awake()
        {
            if (HitBoxComponent == null)
            {
                Debug.Log(DM_ERROR.REFERENCES_NULL);
                return;
            }
            HitBoxComponent.HitBox.OnCollision += (c) => OnHit?.Invoke(c);
        }

        private void SetColor(Renderer renderer, Color color)
        {
            if (renderer.material.shader.name == "Toon")
            {
                renderer.material.SetColor("_BaseColor", color);
            }
            else
            {
                renderer.material.color = color;
            }
        }
    }

}
