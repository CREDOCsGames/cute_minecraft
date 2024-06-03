using PlatformGame.Debugger;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DebugLogUI : MonoBehaviour
{
    public TextMeshProUGUI UI;
    public List<Transform> Transforms;

    void Update()
    {
        UI.text = "";
        foreach (var instanceID in Transforms.Select(x => x.GetInstanceID()))
        {
            if (!DebugWrapper.FrameTagMassages.ContainsKey(instanceID))
            {
                continue;
            }

            foreach (var massage in DebugWrapper.FrameTagMassages[instanceID])
            {
                if (UI.text.Length > 0)
                {
                    UI.text += '\n';
                }

                UI.text += massage.Value;
            }

        }
    }

}