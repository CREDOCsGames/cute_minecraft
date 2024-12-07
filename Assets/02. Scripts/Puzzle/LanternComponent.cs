using Flow;
using Sound;
using UnityEngine;
using Util;

namespace Puzzle
{
    public class LanternComponent : MonoBehaviour
    {
        public int StoneCount
        {
            get => _stone.Count;
            set => _stone.Count = value;
        }

        [Range(0, 100)][SerializeField] private int _clearCount;
        private CountEvent _stone;

        public void SetGameManger()
        {
            GameManager.Lantern = this;
        }

        public void ResetGameManager()
        {
            GameManager.Lantern = null;
        }

        private void CreateStone()
        {
            var stone = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var shader = Shader.Find("Universal Render Pipeline/Lit");
            stone.GetComponent<Renderer>().material = new Material(shader);
            stone.transform.SetParent(transform);
            stone.transform.SetLocalPositionAndRotation(Vector3.down * (2f + _stone.Count), Quaternion.identity);
        }

        private void Awake()
        {
            _stone = new CountEvent();
            _stone.CountReachedEvent += GameManager.PuzzleArea.OnClear;
            _stone.CountAddEvent += CreateStone;
            _stone.CountAddEvent += () => SoundManagerComponent.Instance.PlaySound("Success");
            _stone.UseOneTime();
            _stone.NumberOfGoals = _clearCount;
        }
    }
}