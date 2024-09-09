using PlatformGame.Contents.Puzzle;
using UnityEngine;

public class DisableRange : MonoBehaviour
{
    [SerializeField] bool enable;
    private void OnTriggerEnter(Collider other)
    {
        var f = other.GetComponent<Flower>();
        if (f == null)
        {
            return;
        }
        f.enabled = enable;
    }
}
