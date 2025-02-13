using UnityEngine;

namespace Controller
{
    public class JumpState : IController
    {
        public void HandleInput(Character player)
        {
            if (player.IsGrounded)
            {
                player.ResetJump();
                player.Land();
                player.ChangeController(new LandState());
            }
        }

        public void UpdateState(Character player)
        {

        }
    }

}