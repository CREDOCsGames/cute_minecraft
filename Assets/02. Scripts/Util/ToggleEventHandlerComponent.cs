using UnityEngine;
using UnityEngine.Events;

public class ToggleEventHandlerComponent : MonoBehaviour
{
    public UnityEvent OnTrueEvent;
    public UnityEvent OnFalseEvent;

    public void Toggle(bool toggle)
    {
        if (toggle)
        {
            OnTrueEvent.Invoke();
        }
        else
        {
            OnFalseEvent.Invoke();
        }
    }
}
