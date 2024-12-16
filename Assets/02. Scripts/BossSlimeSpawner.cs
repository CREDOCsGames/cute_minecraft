using UnityEngine;

public class BossSlimeSpawner : MonoBehaviour
{
    public GameObject slimePrefab;
    private GameObject currentSlimeObject;

    public void SpawnAt(Vector3 slimePosition)
    {
        if (currentSlimeObject != null)
        {
            currentSlimeObject.SetActive(false);
        }

        Instantiate(slimePrefab, slimePosition, Quaternion.identity);
    }

    public void SetCurrentSlimeObject(GameObject slimeObject)
    {
        currentSlimeObject = slimeObject;
    }
}