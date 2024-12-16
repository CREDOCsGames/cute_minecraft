namespace Controller
{
    public interface IController
    {
        public void HandleInput(Character player);
        public void UpdateState(Character player);

    }

    public class CanNotControl : IController
    {
        public void HandleInput(Character player)
        {
        }

        public void UpdateState(Character player)
        {
        }
    }

}