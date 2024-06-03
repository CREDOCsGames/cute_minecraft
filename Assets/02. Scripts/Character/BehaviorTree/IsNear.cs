using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class IsNear : Conditional
{

    public SharedTransform TraceTarget;
    public float FieldOfView;
    public override TaskStatus OnUpdate()
    {
        if(!ExistTraceTarget())
        {
            return TaskStatus.Failure;
        }
        if (!IsNearTarget())
        {
            return TaskStatus.Failure;
        }
        return TaskStatus.Success;
    }

    public bool ExistTraceTarget()
    {
        return TraceTarget?.Value != null;
    }

    public bool IsNearTarget()
    {
        var targetPos = TraceTarget.Value.position;
        var myPos = gameObject.transform.position;
        var distance = Vector3.Distance(targetPos, myPos);
        return distance <= FieldOfView;
    }

    public static TaskStatus GetTestSuccess(string message)
    {
        var value = Random.Range(0, 2);
        bool near = value > 0;
        var status = near ? TaskStatus.Success : TaskStatus.Failure;
        Debug.Log($"{message} : {status}");
        return status;
    }
}