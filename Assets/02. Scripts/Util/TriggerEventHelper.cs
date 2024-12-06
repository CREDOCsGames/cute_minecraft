using UnityEngine;

namespace Util
{
    public class TriggerEventHelper : MonoBehaviour
    {
        [SerializeField] private string _tag;

        public void SetParent(Collider collider)
        {
            if (!string.IsNullOrEmpty(_tag) && !collider.CompareTag(_tag))
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