using PlatformGame;
using UnityEngine;
using UnityEngine.UI;

public class TimerHelper : MonoBehaviour
{
    public Slider slider;
    public void SetSliderValue(TimerComponent timer)
    {
        slider.value = 1 - Mathf.Clamp((timer.ElapsedTime / timer.Timeout), 0f, 1f);
    }
}
