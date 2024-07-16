using PlatformGame.Character.Collision;
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
        [SerializeField] GameObject mResultItem;

        [Header("Option")]
        public UnityEvent<Item> OnFailEvent;
        public UnityEvent<Item> OnInputItem;
        public UnityEvent<GameObject> OnOutputItem;
        [Tooltip("요구 개수 초과")]
        [SerializeField] bool mbOvercount;
        [Tooltip("다른 재료 허가")]
        [SerializeField] bool mbOtherInputItem;

        public void OnHit(HitBoxCollision collision)
        {
            var item = collision.Attacker.GetComponent<Item>();
            if (!item)
            {
                return;
            }

            InputItem(item);
        }

        public void ChangeRecipe(List<QuestItem> recipe)
        {
            mRecipe = recipe;
            Init();
        }

        void Init()
        {
            mRecipe.ForEach(x => x.Count = 0);
        }

        void InputItem(Item input)
        {
            var item = mRecipe.Find(x => x.Item.ID == input.ID);
            if (!mbOtherInputItem && item == null)
            {
                OnFailEvent.Invoke(input);
                return;
            }

            if (!mbOvercount && item.IsFull)
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
            var obj = Instantiate(mResultItem);
            OnOutputItem.Invoke(obj);
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
        void Update()
        {
            SetCount();
        }
#endif

    }
}
