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
<<<<<<< HEAD
=======

        public void Reset()
        {
            mActionTimer.Stop();    
        }
>>>>>>> parent of e29ba99d (Merge pull request #148 from 1506022022/main)
    }
}