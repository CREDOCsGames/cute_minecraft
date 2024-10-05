#if UNITY_EDITOR
using PlatformGame.Util;
using UnityEditor;
using UnityEngine;

namespace PlatformGame.Contents.Puzzle
{

    [CustomEditor(typeof(Flower))]
    public class FlowerEditor : Editor
    {
        Flower mFlower;
        Color mPrevColor;

        private void OnEnable()
        {
            mFlower = (Flower)target;
            mPrevColor = mFlower.Color;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();


            if (mPrevColor == mFlower.Color)
            {
                return;
            }

            Colorize.Instance.Invoke(mFlower.Renderers, mFlower.Color);
            mPrevColor = mFlower.Color;
        }
    }
}
#endif