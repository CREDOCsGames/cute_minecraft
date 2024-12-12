using UnityEngine;

namespace Controller
{
    public class MoveState : IPlayerState
    {
        public string Name => "Run";

        public void HandleInput(Character player)
        {
            Vector3 input = new(Input.GetAxisRaw("Horizontal"),
                                0,
                                Input.GetAxisRaw("Vertical"));

            if (input.magnitude == 0)
            {
                player.ChangeState(new IdleState());
                return;
            }

            if (Input.GetButtonDown("Jump"))
            {
                player.ChangeState(new JumpState());
                return;
            }
            if (Input.GetButtonDown("Attack"))
            {
                player.ChangeState(new MeleeAttack());
                return;
            }

            player.Move(input);
        }

        public void UpdateState(Character player)
        {
        }
    }

}