using UnityEngine;
using UnityEngine.UI;

namespace Puzzle
{
    public class MinimapComponent : MonoBehaviour, IInstance, IPuzzleInstance, IDestroyable
    {
        public DataReader DataReader { get; } = new FlowerReader();
        [SerializeField] private GridLayoutGroup _root;
        [SerializeField] private Button _flower;
        [SerializeField] private Image _bg;
        [SerializeField, Range(0, 5)] private byte _viewFace;
        private byte _width;

        public void Init(CubePuzzleDataReader puzzleData)
        {
            _width = puzzleData.Width;
            _root.constraintCount = _width;

            for (int i = 0; i < _width * _width; i++)
            {
                if (_root.transform.childCount <= i)
                {
                    GameObject.Instantiate(_flower, _root.transform);
                }
                else
                {
                    _root.transform.GetChild(i).gameObject.SetActive(true);
                }
            }

            for (byte y = 0; y < _width; y++)
            {
                for (byte x = 0; x < _width; x++)
                {
                    Colorize(new byte[] { x, y, _viewFace, puzzleData.GetElement(x, y, _viewFace) });
                }
            }

            _bg.gameObject.SetActive(true);
        }
        public void InstreamData(byte[] data)
        {
            if (data[2] == _viewFace)
            {
                Colorize(data);
            }
            if (FlowerReader.FLOWER_CREATE.Equals(data))
            {
                // 수정의 개수를 갱신한다
                // 수정의 개수가 n개라면 단계를 넘어간다
                // 추락
            }
        }
        public void SetMediator(IMediatorInstance mediator)
        {
        }
        private void Colorize(byte[] data)
        {
            var index = _width * data[1] + data[0];
            switch (data[3])
            {
                case FlowerReader.EMPTY:
                    _root.transform.GetChild(index).GetComponent<Button>().interactable = false;
                    break;
                case FlowerReader.FLOWER_RED:
                    _root.transform.GetChild(index).GetComponent<Button>().image.color = Color.red;
                    break;
                case FlowerReader.FLOWER_GREEN:
                    _root.transform.GetChild(index).GetComponent<Button>().image.color = Color.green;
                    break;
                default:
                    break;
            }
        }

        public void Destroy()
        {
            for (int i = 0; i < _root.transform.childCount; i++)
            {
                _root.transform.GetChild(i).gameObject.SetActive(false);
            }
            _bg.gameObject.SetActive(false);
        }
    }
}
