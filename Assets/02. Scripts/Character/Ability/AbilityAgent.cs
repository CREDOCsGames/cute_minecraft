namespace PlatformGame.Character.Combat
{
    public class AbilityAgent
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

        public void Use(ActionData actionData)
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
    }
}