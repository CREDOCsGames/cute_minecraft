using UnityEngine;

public class WarpComponent : MonoBehaviour
{
    [SerializeField]
    Transform Target;
    public void WarpTarget()
    {
        Target.position = transform.position;
    }
}
