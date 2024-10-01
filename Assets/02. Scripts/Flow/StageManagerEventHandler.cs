using UnityEngine;
using UnityEngine.Events;

namespace Flow
{
    public class StageManagerEventHandler : MonoBehaviour
    {
        [SerializeField] UnityEvent OnChangeEvent;

        void Awake()
        {
            StageManager.Instance.OnChangeEvent += () => OnChangeEvent.Invoke();
        }
    }
}