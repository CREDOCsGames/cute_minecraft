using BehaviorDesigner.Runtime.Tasks;

public class Patrol : Action
{
    public override void OnStart()
    {

    }

    public override TaskStatus OnUpdate()
    {
        return IsNear.GetTestSuccess($"[{gameObject.name}] Patrol");
    }
}