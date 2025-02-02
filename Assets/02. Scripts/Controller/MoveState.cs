using UnityEngine;

namespace Controller
{
    public class MoveState : IController
    {
        public void HandleInput(Character player)
        {
            Vector3 input = new(Input.GetAxisRaw("Horizontal"),
                                0,
                                Input.GetAxisRaw("Vertical"));

            if (input.magnitude == 0)
            {
                player.Idle();
                player.ChangeController(new IdleState());
                return;
            }
            //if (Input.GetButtonDown("Jump"))
            //{
            //    player.Jump();
            //    player.ChangeController(new JumpState());
            //    return;
            //}
            if (Input.GetButtonDown("Attack"))
            {
                player.Attack();
                player.ChangeController(new MeleeAttack());
                return;
            }

            player.Move(input);
        }

        public void UpdateState(Character player)
        {
        }
    }

}