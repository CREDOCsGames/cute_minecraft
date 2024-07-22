using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame
{
    public class Crafting : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] List<QuestItem> mRecipe;
        public List<QuestItem> Recipe => mRecipe.ToList();

        [Header("Option")]
        public UnityEvent<Item> OnFailEvent;
        public UnityEvent<Item> OnInputItem;
        public UnityEvent OnOutputItem;
        [Tooltip("요구 개수 초과")]
        [SerializeField] bool mbOvercount;
        [Tooltip("다른 재료 허가")]
        [SerializeField] bool mbOtherInputItem;

        public void ChangeRecipe(List<QuestItem> recipe)
        {
            mRecipe = recipe;
            Init();
        }

        void Init()
        {
            mRecipe.ForEach(x => x.Count = 0);
        }

        public void InputItem(Item input)
        {
            var item = mRecipe.Find(x => x.Item.ID == input.ID);
            if (!mbOtherInputItem && item == null)
            {
                OnFailEvent.Invoke(input);
                return;
            }

            if (item.IsFull && !mbOvercount)
            {
                return;
            }

            item.Count++;
            OnInputItem.Invoke(input);

            if (mRecipe.Any(x => !x.IsFull))
            {
                return;
            }
            OutputItem();
            Init();
        }

        void OutputItem()
        {
            OnOutputItem.Invoke();
        }

        void Awake()
        {
            Init();
        }

#if DEVELOPMENT
        string t;
        public void SetCount()
        {
            t = "";
            foreach (var item in mRecipe)
            {
                t += $"({item.Item.name}) {item.Count} / {item.RequiredCount}\n";
            }
            Debugger.DebugWrapper.LogMessage(transform.GetInstanceID(), "07" + t);
        }
#endif

    }
}
