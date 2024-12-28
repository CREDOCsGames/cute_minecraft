using UnityEngine;

namespace Puzzle
{
    public class FlowerPresentation : IPresentation
    {
        private readonly CubeMap<Flower> _cubeMap;
        public FlowerPresentation(CubeMap<Flower> cubeMap)
        {
            _cubeMap = cubeMap;
        }

        public void InstreamData(byte[] data)
        {
            var flower = _cubeMap.GetElements(data[0], data[1], data[2]);
            flower.gameObject.SetActive(true);
            switch (data[3])
            {
                case (byte)Flower.FlowerType.Red:
                    flower.Color = new Color(191f / 255f, 12f / 255f, 255f / 255f);
                    break;
                case (byte)Flower.FlowerType.Green:
                    flower.Color = Color.cyan;
                    break;
                default:
                    flower.gameObject.SetActive(false);
                    break;
            }
        }
    }

}
