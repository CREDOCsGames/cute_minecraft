using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Util
{
    public class Branch : MonoBehaviour
    {
        public bool Condition { get; private set; }
        public UnityEvent TrueEvent;
        public UnityEvent FalseEvent;


        public void MeetCondition()
        {
            Condition = true;
        }

        public void CancleCondition()
        {
            Condition = false;
        }

        public void ToggleCondition()
        {
            if(Condition)
            {
                CancleCondition();
            }
            else
            {
                MeetCondition();
            }
        }

        public void Invoke()
        {
            if (Condition)
            {
                TrueEvent.Invoke();
            }
            else
            {
                FalseEvent.Invoke();
            }
        }
    }

}
