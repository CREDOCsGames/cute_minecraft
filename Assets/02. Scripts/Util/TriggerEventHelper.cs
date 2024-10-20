using UnityEngine;

namespace Util
{
    public class TriggerEventHelper : MonoBehaviour
    {
        [SerializeField] string Tag;

        public void SetParent(Collider collider)
        {
            if (!string.IsNullOrEmpty(Tag) && !collider.CompareTag(Tag))
            {
                return;
            }

            transform.SetParent(collider.transform);
        }

        public void SetParentNull()
        {
            transform.SetParent(null);
        }

        public void SetActiveFalse(Collider collider)
        {
            collider.gameObject.SetActive(false);
        }
    }
}