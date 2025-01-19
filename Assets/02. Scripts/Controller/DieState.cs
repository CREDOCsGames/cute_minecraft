using UnityEngine;

namespace Controller
{
    public class DieState : IController
    {
        private bool _bDead;
        private readonly float _time;
        public DieState()
        {
            _time = Time.time + 0.5f;
        }
        public void HandleInput(Character player)
        {
            if (_time < Time.time && !_bDead && player.IsActionFinished)
            {
                _bDead = true;
                GameObject.Destroy(player.Rigidbody.gameObject);
            }
        }

        public void UpdateState(Character player)
        {
        }
    }

}

