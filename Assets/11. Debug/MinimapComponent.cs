using UnityEngine;
using Util;
namespace Puzzle
{
    public class MinimapComponent : MonoBehaviour
    {
        public Transform Camera;
        public Transform BariaRoot;
        public FlowerPuzzleData Data;
        IndexByteEvent mMinimapViewIndex;

        public void NextView()
        {
            mMinimapViewIndex.Next();
        }

        public void PrevView()
        {
            mMinimapViewIndex.Prev();
        }

        public void CaptureMinimap()
        {
            var pos = BariaRoot.GetChild(mMinimapViewIndex.Value).transform.position;
            Camera.transform.position = pos;
        }

        public void ActiveMinimap()
        {
            Camera.gameObject.SetActive(true);
            BariaRoot.gameObject.SetActive(true);
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            SetMinimap(mMinimapViewIndex.Value);
            CaptureMinimap();
        }

        public void DisActiveMinimap()
        {
            Camera.gameObject.SetActive(false);
            BariaRoot.gameObject.SetActive(false);
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        void SetMinimap(byte index)
        {
            for (var i = 0; i < 6; i++)
            {
                BariaRoot.GetChild(i).gameObject.SetActive(i != index);
            }
        }

        void Awake()
        {
            var Index = new IndexByte(0, 5);
            mMinimapViewIndex = new IndexByteEvent(Index, SetMinimap);

            if (Data.UseMinimap.Equals("True"))
            {
                ActiveMinimap();
            }
            else
            {
                DisActiveMinimap();
            }
        }

    }

}
