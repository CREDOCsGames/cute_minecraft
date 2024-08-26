using PlatformGame;
using UnityEngine;
using UnityEngine.UI;

public class TimerHelper : MonoBehaviour
{
    public Slider slider;
    public void SetSliderValue(Timer timer)
    {
        slider.value = Mathf.Clamp((timer.ElapsedTime / timer.Timeout), 0f, 1f);
    }

    public void SetSliderValueOneminus(Timer timer)
    {
        slider.value = 1 - Mathf.Clamp((timer.ElapsedTime / timer.Timeout), 0f, 1f);
    }
}
