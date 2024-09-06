using PlatformGame.Character.Collision;
using PlatformGame.Util;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformGame.Contents.Puzzle
{
    public interface IColorPiece
    {
        public Color Color { get; set; }
        public List<MeshRenderer> Renderers { get; }
    }

    public class PoisionBox : HitBoxCollider
    {
        [SerializeField] Color mColorA;
        [SerializeField] Color mColorB;

        protected override void Awake()
        {
            base.Awake();
            AddHitEvent(OnHit);

        }

        void OnHit(HitBoxCollision collision)
        {
            RemoveHitEvent(OnHit);
            IsAttacker = true;
            AddHitEvent(OnAttack);
        }

        void OnAttack(HitBoxCollision collision)
        {
            var attacked = collision.Victim.GetComponent<IColorPiece>();
            if (attacked == null)
            {
                return;
            }

            var attacker = collision.Attacker;
            if (attacker == null || !attacker.Equals(Actor))
            {
                return;
            }

            var color = attacked.Color == mColorA ? mColorB : mColorA;
            attacked.Color = color;
        }
    }

}
