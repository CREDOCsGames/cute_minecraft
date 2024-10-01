using PlatformGame.Contents;
using UnityEngine;

namespace PlatformGame
{
    [CreateAssetMenu(menuName = "Custom/Hitbox/ShyBoxManager")]
    public class ShyBoxHelper : UniqueScriptableObject<ShyBoxHelper>
    {
        public void StartShowing(Collider collider)
        {
            var box = collider.GetComponent<ShyBoxComponent>();
            if (box == null)
            {
                return;
            }

            box.StartShowing();
        }

        public void HideBegin(Collider collider)
        {
            var box = collider.GetComponent<ShyBoxComponent>();
            if (box == null)
            {
                return;
            }

            box.HideBegin();
        }

        public void HideEnd(Collider collider)
        {
            var box = collider.GetComponent<ShyBoxComponent>();
            if (box == null)
            {
                return;
            }

            box.HideEnd();
        }

        public void ResetTrigger(Collider collider)
        {
            collider.enabled = false;
            collider.enabled = true;

        }


    }

}