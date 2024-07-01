using UnityEngine;
using UnityEngine.UI;

public class TimerHelper : MonoBehaviour
{
    public Slider slider;
    public void SetSliderValue(StopWatch timer)
    {
        slider.value = 1 - Mathf.Clamp((timer.TotalRunningTime / timer.MaxTimerTime), 0f, 1f);
    }
}
