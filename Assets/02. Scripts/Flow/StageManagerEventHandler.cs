using UnityEngine;
using UnityEngine.Events;

namespace Flow
{
    public class StageManagerEventHandler : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onChangeEvent;

        private void Awake()
        {
            StageManager.Instance.OnChangeEvent += () => _onChangeEvent.Invoke();
        }
    }
}