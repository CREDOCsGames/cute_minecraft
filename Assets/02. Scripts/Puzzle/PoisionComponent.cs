using System.Collections.Generic;
using UnityEngine;
using Battle;
using System.Linq;

namespace Puzzle
{
    public interface IColorPiece
    {
        public Color Color { get; set; }
        public List<MeshRenderer> Renderers { get; }
    }

    public class PoisionComponent : SwitchBoxComponent
    {
        [Header("[Options]")]
        [SerializeField] Color ColorA;
        [SerializeField] Color ColorB;

        public void InvertColor(HitBoxCollision collision)
        {
            var attacked = collision.Victim.GetComponents<IColorPiece>();
            if (!attacked.Any())
            {
                return;
            }

            foreach (var piece in attacked)
            {
                var color = FlowerComponent.CompareColor(piece.Color, ColorA) ? ColorB : ColorA;
                piece.Color = color;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            OnAttack.AddListener(InvertColor);
        }

    }
}