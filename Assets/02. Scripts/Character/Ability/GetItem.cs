using System.Linq;
using UnityEngine;

namespace PlatformGame.Character.Combat
{
    public class GetItem : MonoBehaviour
    {
        public float Range;

        public void Get()
        {
            var foundItems = Item.Instances.Where(x => Vector3.Distance(transform.position, x.transform.position) < Range)
                                           .Where(x => x.GetComponent<Character>().State != CharacterState.Die)
                                           .ToList();

            foreach(var item in foundItems)
            {
                var character = item.GetComponent<Character>();
                PlatformGame.Character.Combat.Destroy.DestroyTo(character);
            }

        }
    }

}