using Battle;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Puzzle
{
    public interface IColorPiece
    {
        public Color Color { get; set; }
        public List<MeshRenderer> Renderers { get; }
    }

    public class PoisonComponent : SwitchBoxComponent
    {
        [Header("[Options]")]
        [SerializeField] Color _colorA;
        [SerializeField] Color _colorB;

        public void InvertColor(HitBoxCollision collision)
        {
            var attacked = collision.Victim.GetComponents<IColorPiece>();
            if (!attacked.Any())
            {
                return;
            }

            foreach (var piece in attacked)
            {
                var color = FlowerComponent.CompareColor(piece.Color, _colorA) ? _colorB : _colorA;
                piece.Color = color;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _onAttack.AddListener(InvertColor);
        }

    }
}