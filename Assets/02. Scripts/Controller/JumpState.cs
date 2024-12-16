using UnityEngine;

namespace Controller
{
    public class JumpState : IController
    {
        public string Name => "Jump";

        public void HandleInput(Character player)
        {
        }

        public void UpdateState(Character player)
        {

            if (player.IsGrounded)
            {
                player.ResetJump();
                player.Land();
                player.ChangeController(new LandState());
            }
        }
    }

}