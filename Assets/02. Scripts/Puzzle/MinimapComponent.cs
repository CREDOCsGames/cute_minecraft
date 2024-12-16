using NW;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Puzzle
{
    public class MinimapAComponent : MonoBehaviour, NW.IInstance
    {
        [SerializeField] private Canvas _root;
        [SerializeField] private Button _flower;
        public NW.DataReader DataReader { get; } = new FlowerReader();

        public void InstreamData(byte[] data)
        {

        }

        public void SetMediator(IMediatorInstance mediator)
        {
        }
    }

}
