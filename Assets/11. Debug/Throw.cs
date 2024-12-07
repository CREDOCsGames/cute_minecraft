using UnityEngine;

public class Throw : MonoBehaviour
{
    public float power = 5f;
    private void OnCollisionEnter(Collision collision)
    {
        var rigid = collision.gameObject.GetComponent<Rigidbody>();
        rigid?.AddForce(transform.up * power);
    }
}
