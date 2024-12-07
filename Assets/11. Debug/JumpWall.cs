using Battle;
using Flow;
using Movement;
using UnityEngine;

public class JumpWall : MonoBehaviour
{
    private MovementComponent movement;
    public MovementAction Action;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent<PCController>(out var con) && con.CharacterState is not PCController.State.Jump)
        {
            movement = GameObject.FindAnyObjectByType<AreaComponent>()?.GetComponent<MovementComponent>();
            con.DoJump(500f);
            if (Action != null && movement != null)
            {
                movement.PlayMovement(Action);
            }

        }
    }
}
