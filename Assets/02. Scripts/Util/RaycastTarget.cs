using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class RaycastTarget : MonoBehaviour
    {
        [SerializeField] UnityEvent<GameObject> _event;
        [SerializeField] float _maxDistance;
        void Update()
        {
            // 마우스 클릭시 레이캐스트 발사
            if (Input.GetMouseButtonDown(0)) // 0: 좌클릭
            {
                RaycastFromMouse();
            }
        }

        void RaycastFromMouse()
        {
            // 카메라에서 마우스 위치로 레이 생성
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // RaycastHit 구조체는 레이캐스트 충돌 정보를 담고 있음
            RaycastHit hit;

            // 레이캐스트 실행: 레이가 충돌한 오브젝트가 있는지 확인
            if (Physics.Raycast(ray, out hit, _maxDistance))
            {
                // 충돌한 오브젝트의 정보 출력
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
