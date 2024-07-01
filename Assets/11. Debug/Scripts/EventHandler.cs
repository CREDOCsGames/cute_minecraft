
using UnityEngine;
using UnityEngine.Events;

public class EventHandler : MonoBehaviour
{
    [SerializeField] UnityEvent Action;

    public void Invoke()
    {
        Action.Invoke();
    }

    private void OnEnable()
    {
        Invoke();
        gameObject.SetActive(false);
    }


}
