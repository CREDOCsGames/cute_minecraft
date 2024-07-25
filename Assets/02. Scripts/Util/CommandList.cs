using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CommandList : MonoBehaviour
{
    public List<UnityEvent> Commands;
    int mIndex;

    public void InvokeCommand()
    {
        if (Commands.Count <= mIndex)
        {
            Debug.LogWarning($"Index out of range : {mIndex}/{Commands.Count}");
            return;
        }

        Commands[mIndex].Invoke();
        mIndex++;
    }
}
