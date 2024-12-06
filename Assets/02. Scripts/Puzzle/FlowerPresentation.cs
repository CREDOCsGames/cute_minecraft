using UnityEngine;

namespace Puzzle
{
    public class FlowerPresentation : IPresentation<Flower>
    {
        public void InstreamData(Flower flower, byte data)
        {
            flower.gameObject.SetActive(true);
            switch (data)
            {
                case (byte)Flower.FlowerType.Red:
                    flower.Color = Color.red;
                    break;
                case (byte)Flower.FlowerType.Green:
                    flower.Color = Color.green;
                    break;
                default:
                    flower.gameObject.SetActive(false);
                    break;
            }
        }
    }

}
