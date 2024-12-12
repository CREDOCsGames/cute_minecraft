using UnityEngine;

namespace Controller
{
    public class LandState : IPlayerState
    {
        public string Name => "Land";

        public void HandleInput(Character player)
        {
            if (player.IsFinishedAction)
            {
                player.ChangeState(new IdleState());
            }
        }

        public void UpdateState(Character player)
        {

        }
    }
}
