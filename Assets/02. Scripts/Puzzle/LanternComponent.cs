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
            get => mStone.Count;
            set => mStone.Count = value;
        }

        [Range(0, 100)] [SerializeField] int ClearCount;
        CountEvent mStone;

        public void SetGameManger()
        {
            GameManager.Lantern = this;
        }

        public void ResetGameManager()
        {
            GameManager.Lantern = null;
        }

        void CreateStone()
        {
            var stone = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var shader = Shader.Find("Universal Render Pipeline/Lit");
            stone.GetComponent<Renderer>().material = new Material(shader);
            stone.transform.SetParent(transform);
            stone.transform.SetLocalPositionAndRotation(Vector3.down * (2f + mStone.Count), Quaternion.identity);
        }

        void Awake()
        {
            mStone = new CountEvent();
            mStone.CountReachedEvent += GameManager.PuzzleArea.OnClear;
            mStone.CountAddEvent += CreateStone;
            mStone.CountAddEvent += () => SoundManagerComponent.Instance.PlaySound("Success");
            mStone.UseOneTime();
            mStone.NumberOfGoals = ClearCount;
        }
    }
}