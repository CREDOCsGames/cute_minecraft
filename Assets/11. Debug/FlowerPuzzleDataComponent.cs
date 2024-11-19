using UnityEngine;
using UnityEngine.UI;

public class FlowerPuzzleDataComponent : MonoBehaviour
{
    public FlowerPuzzleData Data;
    Toggle Toggle => GetComponent<Toggle>();

    public void InitToggleLock()
    {
        Toggle.isOn = Data.UseClearThanLock.Equals("True");
    }

    public void InitToggleBase()
    {
        Toggle.isOn = Data.UseClearConditionThanBaseFace.Equals("True");
    }

    public void InitToggleLinkedClear()
    {
        Toggle.isOn = Data.UseLinkedClear.Equals("True");
    }

    public void InitLink(int data)
    {
        Toggle.isOn = Data.Links[data / 6].Contains(data % 6);
    }

    public void InitMinimap()
    {
        Toggle.isOn = Data.UseMinimap.Equals("True");
    }

}
