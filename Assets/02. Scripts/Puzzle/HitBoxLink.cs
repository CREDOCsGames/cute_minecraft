using Battle;
using System.Linq;

namespace Puzzle
{
    public class HitBoxLink : IDataLink
    {
        public IMediatorInstance Mediator { get; set; }

        public void Link(CubeMap<Flower> map)
        {
            foreach (var index in map.GetAllIndex())
            {
                var flower = map.GetElements(index);
                Link(flower, index.Concat(new byte[] { 0 }).ToArray<byte>());
            }
        }
        private void Link(Flower flower, byte[] data)
        {
            flower.HitBoxComponent.HitBox.OnCollision += (c) => Convert2Vector4Byte(c, data);
        }

        private void Convert2Vector4Byte(HitBoxCollision collision, byte[] data)
        {
            if (collision.Attacker.TryGetComponent<PuzzleAttackBoxComponent>(out var box))
            {
                data[3] = box.Type;
            }
            Mediator?.InstreamDataInstance<FlowerReader>(data);
        }

    }
}
