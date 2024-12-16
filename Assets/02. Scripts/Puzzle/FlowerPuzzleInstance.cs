using UnityEngine;
using Util;

namespace Puzzle
{
    [CreateAssetMenu(menuName = "Custom/Puzzle/FlowerPuzzle")]
    public class FlowerPuzzleInstance : PuzzleInstance, NW.IDestroyable
    {
        [SerializeField] private Flower _flowerPrefab;
        private CubeMap<Flower> _cubeMap;
        private HitBoxLink _dataLink;

        protected override void Instantiate(NW.CubeMapReader puzzleData)
        {
            var instantiator = new Instantiator<Flower>(_flowerPrefab);
            _cubeMap = new CubeMap<Flower>(puzzleData.Width, instantiator);

            float offset = ((puzzleData.Width) / 2f) - 0.5f;
            float offsetY = (puzzleData.BaseTransformSize.x / 2f) - 0.5f;

            foreach (var index in _cubeMap.GetIndex())
            {
                var x = index[0];
                var y = index[1];
                var face = index[2];
                var flower = _cubeMap.GetElements(x, y, face);
                flower.transform.SetParent(puzzleData.BaseTransform);
                _dataLink.Link(flower, new[] { x, y, face, (byte)0 });
                switch (face)
                {
                    case (byte)Face.front:
                        flower.transform.localPosition = new Vector3(x - offset, y - offset, 0) + Vector3.forward * ((offsetY) + 0.5f);
                        flower.transform.localRotation = Quaternion.Euler(90, 0, 0);
                        break;
                    case (byte)Face.back:
                        flower.transform.localPosition = Quaternion.Euler(0, 0, 180) * new Vector3(x - offset, y - offset, 0) + Vector3.back * ((offsetY) + 0.5f);
                        flower.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                        break;
                    case (byte)Face.top:
                        flower.transform.localPosition = Quaternion.Euler(270, 0, 0) * new Vector3(x - offset, y - offset, 0) + Vector3.up * ((offsetY) + 0.5f);
                        break;
                    case (byte)Face.bottom:
                        flower.transform.localPosition = Quaternion.Euler(90, 0, 0) * new Vector3(x - offset, y - offset, 0) + Vector3.down * ((offsetY) + 0.5f);
                        flower.transform.localRotation = Quaternion.Euler(180, 0, 0);
                        break;
                    case (byte)Face.left:
                        flower.transform.localPosition = Quaternion.Euler(0, 90, 0) * new Vector3(x - offset, y - offset, 0) + Vector3.left * ((offsetY) + 0.5f);
                        flower.transform.localRotation = Quaternion.Euler(0, 0, 90);
                        break;
                    case (byte)Face.right:
                        flower.transform.localPosition = Quaternion.Euler(0, 270, 0) * new Vector3(x - offset, y - offset, 0) + Vector3.right * ((offsetY) + 0.5f);
                        flower.transform.localRotation = Quaternion.Euler(0, 0, -90);
                        break;
                    default:
                        break;
                };

            }

        }

        protected override void SetDataLink(out IDataLink dataLink)
        {
            _dataLink = new HitBoxLink();
            dataLink = _dataLink;
        }

        protected override void SetPresentation(out IPresentation presentation)
        {
            presentation = new FlowerPresentation(_cubeMap);
        }

        protected override void SetDataReader(out NW.DataReader reader)
        {
            reader = new FlowerReader();
        }

        public void Destroy()
        {
            foreach (var obj in _cubeMap.Elements)
            {
                Destroy(obj);
            }
            _cubeMap = null;
        }
    }
}

