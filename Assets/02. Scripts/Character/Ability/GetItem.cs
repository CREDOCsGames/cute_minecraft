using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlatformGame.Character.Combat
{
    public class GetItem : MonoBehaviour
    {
        public float Range;
        Dictionary<Item, int> mInventory = new();
        public void Get()
        {
            var foundItems = Item.Instances.Where(x => Vector3.Distance(transform.position, x.transform.position) < Range)
                                           .Where(x => x.GetComponent<Character>().State != CharacterState.Die)
                                           .ToList();

            foreach (var item in foundItems)
            {
                var character = item.GetComponent<Character>();
                if (mInventory.ContainsKey(item))
                {
                    mInventory[item]++;
                }
                else
                {
                    mInventory.Add(item, 1);
                }

                PlatformGame.Character.Combat.Destroy.DestroyTo(character);
            }

        }

        public void Push(Crafting crafting)
        {
            foreach (var item in mInventory.ToList())
            {
                for (int i = 0; i < item.Value; i++)
                {
                    crafting.InputItem(item.Key);
                }
                mInventory.Remove(item.Key);
            }
        }
    }

}