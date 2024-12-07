using Battle;
using System;

namespace Puzzle
{
    public class HitBoxLink : IDataLink<Flower>
    {
        public event Action<byte[]> OnInteraction;

        public void Link(Flower flower, byte[] data)
        {
            flower.HitBoxComponent.HitBox.OnCollision += (c) => Convert2Vector4Byte(c, data);
        }

        private void Convert2Vector4Byte(HitBoxCollision collision, byte[] data)
        {
            if (collision.Attacker.TryGetComponent<PuzzleAttackBoxComponent>(out var box))
            {
                data[3] = box.Type;
            }
            OnInteraction.Invoke(data);
        }
    }

}
