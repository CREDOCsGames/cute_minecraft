namespace Controller
{
    public interface IPlayerState
    {
        public string Name { get; }
        public void HandleInput(Character player);
        public void UpdateState(Character player);

    }
}