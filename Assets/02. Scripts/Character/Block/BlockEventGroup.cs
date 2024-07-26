using PlatformGame.Character;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BlockEventGroup : MonoBehaviour
{
    [SerializeField] List<GameObject> Blocks;
    UnityAction<BlockEvent> mAction;

    public void SetEnable(bool enable)
    {
        mAction = (block) => { block.SetEnable(enable); };
        Invoke();
    }

    public void InvokeEvent()
    {
        foreach (var block in Blocks)
        {
            var be = block.GetComponent<BlockEvent>();
            if (be == null || !be.IsEnable)
            {
                continue;
            }
            be.ButtonEvent.Invoke();
        }
    }

    void Invoke()
    {
        foreach (var block in Blocks)
        {
            var be = block.GetComponent<BlockEvent>();
            if (be == null)
            {
                continue;
            }
            mAction.Invoke(be);
        }
    }
}
