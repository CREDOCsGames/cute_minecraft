using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CommandList : MonoBehaviour
{
    int mIndex;

    [Header("References")]
    [SerializeField] List<UnityEvent> Commands;

    [Header("Options")]
    [SerializeField] bool bClamp;

    public void InvokeCommand()
    {
        if (bClamp && Commands.Count <= mIndex)
        {
            mIndex = 0;
        }

        if (Commands.Count <= mIndex)
        {
            Debug.Log($"Index out of range : {mIndex}/{Commands.Count} {name}");
            return;
        }

        Commands[mIndex].Invoke();
        mIndex++;
    }

    public void Clear()
    {
        Commands.Clear();
    }

    public void Init()
    {
        mIndex = 0;
    }
}
