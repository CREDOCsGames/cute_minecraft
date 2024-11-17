using Battle;
using System;
using UnityEngine;

namespace Puzzle
{
    public class HitBoxLink : IDataLink<Flower>
    {
        public event Action<byte[]> OnInteraction;

        public void Link(Flower flower, byte[] data)
        {
            flower.HitBoxComponent.HitBox.OnCollision += (c) => Convert2Vector4Byte(c, data);
        }

        void Convert2Vector4Byte(HitBoxCollision collision, byte[] data)
        {
            if (collision.Attacker.TryGetComponent<PuzzleAttackBoxComponent>(out var box))
            {
                data[3] = box.Type;
            }
            Debug.Log("D");
            OnInteraction.Invoke(data);
        }
    }

}
