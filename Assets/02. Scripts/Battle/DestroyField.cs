using UnityEngine;

public class DestroyField : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag.Equals("Ground"))
        {
            return;
        }
        Destroy(collision.gameObject);
    }
}
