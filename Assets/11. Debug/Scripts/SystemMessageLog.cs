using PlatformGame.Debugger;
using Puzzle;
using UnityEngine;

[CreateAssetMenu(menuName = "Log/SystemMessage")]
public class SystemMessageLog : ScriptableObject, IInstance
{
    public DataReader DataReader => SystemReader.Instance;

    private void PrintSystemMessage(byte[] data)
    {
        Debug.Log($"[System] {DebugLog.GetStrings(data)}");
    }
    private void PrintFlowerMessage(byte[] data)
    {
        Debug.Log($"[Puzzle] {DebugLog.GetStrings(data)}");
    }
    private void PrintMonsterMessage(byte[] data)
    {
        Debug.Log($"[Monster] {DebugLog.GetStrings(data)}");
    }

    public void SetMediator(IMediatorInstance mediator)
    {
    }
    public void InstreamData(byte[] data)
    {
        PrintSystemMessage(data);
    }
}
