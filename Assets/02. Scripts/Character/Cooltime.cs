using Flow;

namespace Character
{
    public class Cooltime
    {
        private readonly Timer _actionTimer = new();

        public bool IsAction
        {
            get
            {
                _actionTimer.Tick();
                return _actionTimer.IsStart;
            }
        }

        public void UseAbility(ActionData actionData)
        {
            if (_actionTimer.IsStart)
            {
                return;
            }

            if (actionData.ActionDelay <= 0)
            {
                return;
            }

            _actionTimer.SetTimeout(actionData.ActionDelay);
            _actionTimer.Start();
        }

        public void Reset()
        {
            _actionTimer.Stop();
        }
    }
}