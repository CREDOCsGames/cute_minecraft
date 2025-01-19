using UnityEngine;

namespace Controller
{
    public class LandState : IController
    {
        private float _holdTime = 0.1f;
        private float _endTime = -1f;
        public void HandleInput(Character player)
        {
            if (_endTime != -1f && _endTime < Time.time)
            {
                player.ChangeController(new IdleState());
                return;
            }

            if (_endTime == -1f && player.IsActionFinished)
            {
                player.Idle();
                _endTime = Time.time + _holdTime;
            }
        }

        public void UpdateState(Character player)
        {

        }
    }
}
