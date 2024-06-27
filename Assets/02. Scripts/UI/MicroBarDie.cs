using Microlight.MicroBar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MicroBarDie : MonoBehaviour
{
    public UnityEvent DieEvent;
    public UnityEvent TakeDamageEvent;
    [SerializeField] MicroBar bar;

    private void Awake()
    {
        bar.OnCurrentValueChange += Die;
    }

    public void Die(MicroBar bar)
    {
        if (bar.CurrentValue > 0)
        {
            TakeDamageEvent.Invoke();
            return;
        }
        DieEvent.Invoke();
    }
}
