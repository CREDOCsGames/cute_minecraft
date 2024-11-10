using UnityEngine;

namespace Puzzle
{
    public class FlowerPresentation : IPresentation<Flower>
    {
        public void UpstreamData(Flower flower, byte data)
        {
            switch (data)
            {
                case 1:
                    flower.Color = Color.red;
                    break;
                case 2:
                    flower.Color = Color.green;
                    break;
                default:
                    flower.gameObject.SetActive(false);
                    break;
            }
        }
    }

}
