using UnityEngine;

namespace Util
{
    public class WarpComponent : MonoBehaviour
    {
        [SerializeField] Transform Target;

        public void WarpTarget()
        {
            Target.position = transform.position;
        }
    }
}