using UnityEngine;

public class Warp : MonoBehaviour
{
    [SerializeField]
    Transform Target;
    public void WarpTarget()
    {
        Target.position = transform.position;
    }
}
