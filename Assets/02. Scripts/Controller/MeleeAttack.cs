namespace Controller
{
    public class MeleeAttack : IController
    {
        public string Name => "Attack";

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
