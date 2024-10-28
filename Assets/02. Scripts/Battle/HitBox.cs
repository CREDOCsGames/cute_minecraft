using System;
using UnityEngine;

namespace Battle
{
    public struct HitBoxCollision
    {
        public Transform Victim;
        public Transform Attacker;
    }

    public interface IHitBox
    {
        public HitBox HitBox { get; }
    }

    [Serializable]
    public class HitBox : AttackBox
    {
        public HitBox(Transform actor) : base(actor)
        {
        }
    }
}
