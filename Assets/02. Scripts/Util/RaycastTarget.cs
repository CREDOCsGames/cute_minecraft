using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class RaycastTarget : MonoBehaviour
    {
        [SerializeField] private UnityEvent<GameObject> _event;
        [SerializeField] private float _maxDistance;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastFromMouse();
            }
        }

        private void RaycastFromMouse()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, _maxDistance))
            {
                GameObject hitObject = hit.collider.gameObject;
                _event.Invoke(hitObject);
            }

        }

        public void SetPositoin(GameObject obj)
        {
            transform.position = obj.transform.position;
        }
    }

}
