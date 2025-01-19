namespace Controller
{
    public class HitState : IController
    {
        public string Name => "Hit";

        public void HandleInput(Character player)
        {
            if (player.IsActionFinished)
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
