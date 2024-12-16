using UnityEngine;

namespace Controller
{
    public class LandState : IController
    {
        public void HandleInput(Character player)
        {
            if (player.IsFinishedAction)
            {
                player.Idle();
                player.ChangeController(new IdleState());
            }
        }

        public void UpdateState(Character player)
        {

        }
    }
}
