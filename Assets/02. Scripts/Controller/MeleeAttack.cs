using UnityEngine;

namespace Controller
{
    public class MeleeAttack : IController
    {
        private readonly float _enterTime = Time.time + 0.2f;
        private float _minStayTime;

        public void HandleInput(Character player)
        {
            if (Time.time < _enterTime)
            {
                return;
            }
            if (_minStayTime != 0 && _minStayTime < Time.time)
            {
                player.ChangeController(new IdleState());
                return;
            }
            if (player.IsActionFinished)
            {
                player.Idle();
                _minStayTime = Time.time + 0.1f;
            }
        }

        public void UpdateState(Character player)
        {
        }
    }
}
