using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Attack : Action
{
    public override void OnStart()
    {
    }

    public override TaskStatus OnUpdate()
    {
        Debug.Log($"Atatck : {gameObject.name}");
        return TaskStatus.Success;
    }
}