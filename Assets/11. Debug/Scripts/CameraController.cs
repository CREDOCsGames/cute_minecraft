using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f;     // 카메라 이동 속도
    public float heightSpeed = 5f;    // 카메라 높이 조정 속도
    public float lookSpeed = 3f;      // 카메라 회전 속도
    public float maxLookAngle = 80f;  // 카메라의 최대 상하 회전 각도

    private float pitch = 0f;         // 카메라의 상하 회전 각도
    private float yaw = 0f;           // 카메라의 좌우 회전 각도

    void Update()
    {
        // 카메라 이동
        float horizontal = Input.GetAxis("Horizontal");  // A/D 또는 좌/우 화살표
        float vertical = Input.GetAxis("Vertical");      // W/S 또는 위/아래 화살표
        float heightChange = 0f;

        if (Input.GetKey(KeyCode.Q))
        {
            heightChange = -heightSpeed * Time.deltaTime; // Q 키를 눌렀을 때 카메라 내려가기
        }
        else if (Input.GetKey(KeyCode.E))
        {
            heightChange = heightSpeed * Time.deltaTime;  // E 키를 눌렀을 때 카메라 올리기
        }

        // 이동 방향 계산
        Vector3 moveDirection = new Vector3(horizontal, heightChange, vertical);
        moveDirection = transform.TransformDirection(moveDirection);  // 월드 공간에서 로컬 공간으로 변환

        // 카메라 이동
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // 마우스 이동에 따른 카메라 회전
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yaw += mouseX * lookSpeed;  // 좌우 회전
        pitch -= mouseY * lookSpeed;  // 상하 회전
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);  // 상하 회전 각도 제한

        // 카메라 회전 적용
        transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
