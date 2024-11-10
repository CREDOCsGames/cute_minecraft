using UnityEngine;
using Util;

namespace Puzzle
{
    [CreateAssetMenu(menuName = "Custom/Puzzle/FlowerPuzzle")]
    public class FlowerPuzzleInstance : PuzzleInstance<Flower>
    {
        [SerializeField] Flower _flowerPrefab;
        public override byte Width => (byte)PuzzleMap.Column;
        [SerializeField] MatrixBool _puzzleMap;

        public override MatrixBool PuzzleMap => _puzzleMap;

        protected override void Instantiate(out CubeMap<Flower> cubeMap)
        {
            var instantiator = new Instantiator<Flower>(_flowerPrefab);
            cubeMap = new CubeMap<Flower>(Width, instantiator);

            float offset = ((Width) / 2f) - 0.5f;

            foreach (var index in cubeMap.GetIndex())
            {
                var x = index[0];
                var y = index[1];
                var face = index[2];
                var flower = cubeMap.GetElements(x, y, face);

                switch (face)
                {
                    case (byte)Face.front:
                        flower.transform.localPosition = new Vector3(x - offset, y - offset, 0) + Vector3.forward * ((offset) + 0.5f);
                        flower.transform.localRotation = Quaternion.Euler(90, 0, 0);
                        break;
                    case (byte)Face.back:
                        flower.transform.localPosition = Quaternion.Euler(0, 0, 180) * new Vector3(x - offset, y - offset, 0) + Vector3.back * ((offset) + 0.5f);
                        flower.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                        break;
                    case (byte)Face.top:
                        flower.transform.localPosition = Quaternion.Euler(270, 0, 0) * new Vector3(x - offset, y - offset, 0) + Vector3.up * ((offset) + 0.5f);
                        break;
                    case (byte)Face.bottom:
                        flower.transform.localPosition = Quaternion.Euler(90, 0, 0) * new Vector3(x - offset, y - offset, 0) + Vector3.down * ((offset) + 0.5f);
                        flower.transform.localRotation = Quaternion.Euler(180, 0, 0);
                        break;
                    case (byte)Face.left:
                        flower.transform.localPosition = Quaternion.Euler(0, 90, 0) * new Vector3(x - offset, y - offset, 0) + Vector3.left * ((offset) + 0.5f);
                        flower.transform.localRotation = Quaternion.Euler(0, 0, 90);
                        break;
                    case (byte)Face.right:
                        flower.transform.localPosition = Quaternion.Euler(0, 270, 0) * new Vector3(x - offset, y - offset, 0) + Vector3.right * ((offset) + 0.5f);
                        flower.transform.localRotation = Quaternion.Euler(0, 0, -90);
                        break;
                    default:
                        break;
                };
            }

        }

        protected override void SetDataLink(out IDataLink<Flower> dataLink)
        {
            dataLink = new HitBoxLink();
        }

        protected override void SetPresentation(out IPresentation<Flower> presentation)
        {
            presentation = new FlowerPresentation();
        }
    }
}

