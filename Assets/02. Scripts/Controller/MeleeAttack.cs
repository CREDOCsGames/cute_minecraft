namespace Controller
{
    public class MeleeAttack : IPlayerState
    {
        public string Name => "Attack";

        public void HandleInput(Character player)
        {
            if (player.IsFinishedAction)
            {
                player.ChangeState(new IdleState());
            }
        }

        public void UpdateState(Character player)
        {
        }
    }
}
