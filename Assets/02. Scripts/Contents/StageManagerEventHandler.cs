using PlatformGame.Contents;
using UnityEngine;
using UnityEngine.Events;

public class StageManagerEventHandler : MonoBehaviour
{
    [SerializeField] UnityEvent OnChangeEvent;

    void Awake()
    {
        StageManager.Instance.OnChangeEvent += () => OnChangeEvent.Invoke();
    }
}