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
            var face = (Face)data[2];
            if (face is Face.bottom)
            {
                flower.gameObject.SetActive(false);
                return;
            }

            flower.gameObject.SetActive(true);
            switch (data[3])
            {
                case (byte)Flower.Type.Red:
                    flower.Color = new Color(191f / 255f, 12f / 255f, 255f / 255f);
                    break;
                case (byte)Flower.Type.Green:
                    flower.Color = Color.cyan;
                    break;
                default:
                    flower.gameObject.SetActive(false);
                    break;
            }
        }
    }

}
