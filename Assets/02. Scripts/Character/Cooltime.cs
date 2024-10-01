using Flow;

namespace Character
{
    public class Cooltime
    {
        readonly Timer mActionTimer = new();

        public bool IsAction
        {
            get
            {
                mActionTimer.Tick();
                return mActionTimer.IsStart;
            }
        }

        public void UseAbility(ActionData actionData)
        {
            if (mActionTimer.IsStart)
            {
                return;
            }

            if (actionData.ActionDelay <= 0)
            {
                return;
            }

            mActionTimer.SetTimeout(actionData.ActionDelay);
            mActionTimer.Start();
        }

        public void Reset()
        {
            mActionTimer.Stop();
        }
    }
}