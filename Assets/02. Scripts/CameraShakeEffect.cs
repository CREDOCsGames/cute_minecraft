using UnityEngine;

public class CameraShakeEffect : MonoBehaviour
{
    [SerializeField] private float Shakeamount = 0.02f;
    private Vector3 Pos;

    void Awake()
    {
        Pos = transform.position;
    }

    void Update()
    {
        transform.position = Pos + Random.insideUnitSphere * Shakeamount;
    }
}
