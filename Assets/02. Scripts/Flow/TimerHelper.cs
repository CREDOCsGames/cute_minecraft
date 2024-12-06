using UnityEngine;
using UnityEngine.UI;

namespace Flow
{
    public class TimerHelper : MonoBehaviour
    {
        [SerializeField] private Slider _slider;

        public void SetSliderValue(Timer timer)
        {
            _slider.value = Mathf.Clamp((timer.ElapsedTime / timer.Timeout), 0f, 1f);
        }

        public void SetSliderValueOneminus(Timer timer)
        {
            _slider.value = 1 - Mathf.Clamp((timer.ElapsedTime / timer.Timeout), 0f, 1f);
        }
    }
}