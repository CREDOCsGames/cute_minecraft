using UnityEngine;

namespace Puzzle
{
    public class DisableRange : MonoBehaviour
    {
        [SerializeField] private bool _enable;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<FlowerComponent>(out var f))
            {
                return;
            }

            f.enabled = _enable;
        }
    }
}