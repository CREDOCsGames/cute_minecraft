using UnityEngine;

namespace Controller
{
    public class Vector3IntUtil
    {
        public static Vector3Int Convert(Vector3 value)
        {
            return new Vector3Int((int)value.x, (int)value.y, (int)value.z);
        }
    }
    public class BattleUtil
    {
        public static bool IsNear(Transform A, Transform B)
        {
            return Vector3Int.Distance(Vector3IntUtil.Convert(A.transform.position), Vector3IntUtil.Convert(B.transform.position)) <= 2f;
        }
    }
    public class MonsterState : IController
    {
        private Transform _traceTarget;
        public void StartTrace(Transform target)
        {
            _traceTarget = target;
        }

        public void HandleInput(Character player)
        {
            if (_traceTarget == null)
            {
                if (player.State is not CharacterState.Idle)
                {
                    player.Idle();
                }
                return;
            }

            if (player.State is CharacterState.Attack && player.IsFinishedAction)
            {
                player.Idle();
                return;
            }

            if (player.State is CharacterState.Run)
            {
                player.Idle();
                return;
            }

            if (BattleUtil.IsNear(player.Transform, _traceTarget))
            {
                if (player.State is CharacterState.Idle)
                {
                    player.Attack();
                }
                return;
            }
            else
            {
                if (player.State is CharacterState.Idle)
                {
                    player.Move((_traceTarget.position - player.Transform.position).normalized);
                }
                return;
            }

        }

        public void UpdateState(Character player)
        {
        }
    }
}
