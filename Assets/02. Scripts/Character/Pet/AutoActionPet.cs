using UnityEngine.Events;

namespace PlatformGame.Character
{
    public class AutoActionPet : Role
    {
        public float ActionDelay;
        Timer mTimer;
        public UnityEvent Action;

        protected override void Awake()
        {
            base.Awake();
            mTimer = new Timer();
            mTimer.SetTimeout(ActionDelay);
            mTimer.OnTimeoutEvent += (a) => Action.Invoke();
            mTimer.OnTimeoutEvent += (a) => mTimer.Start();
        }

        protected override void Update()
        {
            base.Update();
            mTimer.Tick();
        }

        void Start()
        {
            mTimer.Start();
        }

        void OnDisable()
        {
            mTimer.Stop();
        }
    }

}