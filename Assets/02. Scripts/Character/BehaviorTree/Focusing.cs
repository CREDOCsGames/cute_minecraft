using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace PlatformGame.Character.BehaviorTree
{
    public abstract class Focusing : Action
    {
        public SharedTransform TraceTarget;
        protected Character mMe;

        public override void OnAwake()
        {
            mMe = gameObject.GetComponent<Character>();
            Debug.Assert(mMe != null, $"{gameObject.name} is not a character");
        }

        public override TaskStatus OnUpdate()
        {
            Debug.Assert(TraceTarget != null, $"TraceTarget is null in {Owner.name}");
            TraceTarget.Value = GetTargetInFocus();
            return TraceTarget.Value ? TaskStatus.Success : TaskStatus.Failure;
        }

        protected abstract Transform GetTargetInFocus();

    }

}
