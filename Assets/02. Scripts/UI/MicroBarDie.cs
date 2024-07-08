using Microlight.MicroBar;
using UnityEngine;
using UnityEngine.Events;

public class MicroBarDie : MonoBehaviour
{
    public UnityEvent DieEvent;
    public UnityEvent TakeDamageEvent;
    bool mbDie;
    [SerializeField] MicroBar bar;

    public void Die(MicroBar bar)
    {
        if (mbDie)
        {
            mbDie = bar.CurrentValue <= 0;
            return;
        }

        mbDie = bar.CurrentValue <= 0;
        if (!mbDie)
        {
            TakeDamageEvent.Invoke();
            return;
        }

        DieEvent.Invoke();
    }

    void Awake()
    {
        bar.OnCurrentValueChange += Die;
    }

    void OnDestroy()
    {
        bar.OnCurrentValueChange -= Die;
    }

}
