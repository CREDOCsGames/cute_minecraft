using UnityEngine;

namespace Controller
{
    public class IdleState : IController
    {
        public void HandleInput(Character player)
        {
            Vector3 input = new(Input.GetAxisRaw("Horizontal"),
                                0,
                                Input.GetAxisRaw("Vertical"));

            if (input.magnitude != 0)
            {
                player.ChangeController(new MoveState());
                return;
            }

            if (Input.GetButtonDown("Jump"))
            {
                player.Jump();
                player.ChangeController(new JumpState());
                return;
            }
            if (Input.GetButtonDown("Attack"))
            {
                player.Attack();
                player.ChangeController(new MeleeAttack());
                return;
            }
        }

        public void UpdateState(Character player)
        {
        }
    }

}