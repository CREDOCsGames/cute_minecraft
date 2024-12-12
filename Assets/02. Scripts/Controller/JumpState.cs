using UnityEngine;

namespace Controller
{
    public class JumpState : IPlayerState
    {
        public string Name => "Jump";

        public void HandleInput(Character player)
        {
        }

        public void UpdateState(Character player)
        {
            if (!player.IsJumping)
            {
                player.Jump();
            }

            if (player.IsGrounded)
            {
                player.ResetJump();
                player.ChangeState(new LandState());
            }
        }
    }

}