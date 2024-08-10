using PlatformGame.Character.Combat;
using System.Linq;
using UnityEngine;

namespace PlatformGame
{
    public class InventoryRadar : MonoBehaviour
    {
        [SerializeField] Transform myTransform;
        public void Get()
        {
            var inventory = Radar(myTransform);
            inventory.Get();
        }

        private static GetItem Radar(Transform myTransform)
        {
            return GetItem.Instances.OrderBy(x => Vector3.Distance(myTransform.position, x.transform.position)).First();
        }

        public void Push(Crafting crafting)
        {
            var inventory = Radar(myTransform);
            inventory.Push(crafting);
        }

        void Awake()
        {
            Debug.Assert(myTransform != null, $"Null references in InventoryRadar : {gameObject.name}");
        }
    }

}
