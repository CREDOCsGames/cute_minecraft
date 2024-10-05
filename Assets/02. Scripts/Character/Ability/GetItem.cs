using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlatformGame.Character.Combat
{
    public class GetItem : MonoBehaviour
    {
        static readonly List<GetItem> mInstances = new();
        public static IEnumerable<GetItem> Instances
        {
            get
            {
                Debug.Assert(mInstances.Count != 0, "There is not a single instance of GetItem.");
                return mInstances.ToList();
            }
        }
        public float Range;
        readonly Dictionary<Item, int> mInventory = new();
        public void Get()
        {
            var foundItems = Item.Instances.Where(x => Vector3.Distance(transform.position, x.transform.position) < Range)
                                           .Where(x => x.GetComponent<Character>().State != CharacterState.Die)
                                           .ToList();

            foreach (var item in foundItems)
            {
                var character = item.GetComponent<Character>();
                if (!mInventory.TryAdd(item, 1))
                {
                    mInventory[item]++;
                }

                PlatformGame.Character.Combat.Destroy.DestroyTo(character);
            }

        }

        public void Push(Crafting crafting)
        {
            foreach (var item in mInventory.ToList())
            {
                for (var i = 0; i < item.Value; i++)
                {
                    crafting.InputItem(item.Key);
                }
                mInventory.Remove(item.Key);
            }
        }
        void Awake()
        {
            mInstances.Add(this);
        }
        void OnDestroy()
        {
            mInstances.Remove(this);
        }
    }

}