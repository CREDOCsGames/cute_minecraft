using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using PlatformGame.Character.Combat;
using UnityEngine;

namespace PlatformGame.Character.BehaviorTree
{
    public class Trace : Action
    {
        public SharedTransform TraceTarget;
        public ActionData MoveForward;
        public ActionData MoveBackward;
        public ActionData MoveRight;
        public ActionData MoveLeft;
        Character mMe;

        public override void OnAwake()
        {
            mMe = GetComponent<Character>();
            Debug.Assert(mMe != null, $"{gameObject.name} is not a character");
        }

        public override TaskStatus OnUpdate()
        {
            Debug.Assert(TraceTarget?.Value, $"TraceTarget is null in {Owner.name}");
            var goal = TraceTarget.Value.transform.position;
            var dir = (goal - transform.position).normalized;

            int countX = Mathf.Abs((int)(dir.x * 10));
            for (int i = 0; i < countX; i++)
            {
                var action = dir.x > 0 ? MoveRight : MoveLeft;
                mMe.DoAction(action.ID);
            }

            int countZ = Mathf.Abs((int)(dir.z * 10));
            for (int i = 0; i < countZ; i++)
            {
                var action = dir.z > 0 ? MoveForward : MoveBackward;
                mMe.DoAction(action.ID);
            }
            return TaskStatus.Success;
        }

    }

}