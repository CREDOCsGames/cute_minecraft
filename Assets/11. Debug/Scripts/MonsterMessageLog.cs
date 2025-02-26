using PlatformGame.Debugger;
using Puzzle;
using UnityEngine;

[CreateAssetMenu(menuName = "Log/MonsterMessage")]
public class MonsterMessageLog : ScriptableObject, IInstance
{
    public DataReader DataReader => MonsterReader.Instance;

    private void PrintMonsterMessage(byte[] data)
    {
        Debug.Log($"[Monster] {DebugLog.GetStrings(data)}");
    }
    public void SetMediator(IMediatorInstance mediator)
    {
    }
    public void InstreamData(byte[] data)
    {
        PrintMonsterMessage(data);
    }
}
