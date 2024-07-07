using System;
using UnityEngine;

namespace PlatformGame
{
    [Serializable]
    public class QuestItem
    {
        [SerializeField] Item mItem;
        public Item Item => mItem;
        [SerializeField] byte mRequiredCount;
        public byte RequiredCount => mRequiredCount;
        byte mCount;
        public byte Count
        {
            get => mCount;
            set => mCount = (byte)Mathf.Clamp(value, 0, 255);
        }
        public bool IsFull => mRequiredCount <= Count;
    }

    public class Item : MonoBehaviour
    {
        [SerializeField] int mID;
        public int ID => mID;
    }

}
