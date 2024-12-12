using UnityEngine;

namespace Controller
{
    public class IdleState : IPlayerState
    {
        public string Name => "Idle";

        public void HandleInput(Character player)
        {
            Vector3 input = new(Input.GetAxisRaw("Horizontal"),
                                0,
                                Input.GetAxisRaw("Vertical"));

            if (input.magnitude != 0)
            {
                player.ChangeState(new MoveState());
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
        }

        public void UpdateState(Character player)
        {
        }
    }

}