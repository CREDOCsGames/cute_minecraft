using UnityEngine;

namespace Controller
{
    public class GoToState : IController
    {
        private static float _minDistance = 0.1f;
        private Vector3 _goal;

        public GoToState(Vector3 goal)
        {
            _goal = goal;
        }
        public void HandleInput(Character player)
        {
            if (Vector3.Distance(player.Position, _goal) < _minDistance)
            {
                player.Idle();
                player.ChangeController(new IdleState());
            }
            else
            {
                player.Move(_goal - player.Position);
            }
        }

        public void UpdateState(Character player)
        {
        }

    }

}
