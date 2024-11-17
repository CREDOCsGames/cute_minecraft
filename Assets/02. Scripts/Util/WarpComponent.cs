using UnityEngine;

namespace Util
{
    public class WarpComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        public void WarpTarget()
        {
            _target.position = transform.position;
        }
    }
}