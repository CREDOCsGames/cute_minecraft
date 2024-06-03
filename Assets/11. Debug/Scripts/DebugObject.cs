using UnityEngine;

public class DebugObject : MonoBehaviour
{
    void Awake()
    {
#if DEVELOPMENT
        gameObject.SetActive(true);
#else
        gameObject.SetActive(false);
#endif
    }
}