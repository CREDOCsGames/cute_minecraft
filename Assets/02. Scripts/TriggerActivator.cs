using UnityEngine;

public class TriggerActivator : MonoBehaviour
{
    public GameObject targetObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetObject.SetActive(true);
        }
    }
}