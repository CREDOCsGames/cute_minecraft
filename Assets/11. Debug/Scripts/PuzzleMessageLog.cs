using PlatformGame.Debugger;
using Puzzle;
using UnityEngine;

[CreateAssetMenu(menuName ="Log/PuzzleMessage")]
public class PuzzleMessageLog : ScriptableObject, IInstance
{
    public DataReader DataReader => FlowerReader.Instance;
    private void PrintFlowerMessage(byte[] data)
    {
        Debug.Log($"[Puzzle] {DebugLog.GetStrings(data)}");
    }
    public void SetMediator(IMediatorInstance mediator)
    {
    }
    public void InstreamData(byte[] data)
    {
        PrintFlowerMessage(data);
    }
}
