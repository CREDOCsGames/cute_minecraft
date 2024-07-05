using UnityEngine;
using UnityEngine.Events;

public class StopWatch : MonoBehaviour
{
    public UnityEvent<StopWatch> OnTimerStart;
    public UnityEvent<StopWatch> OnTimerStop;
    public UnityEvent<StopWatch> OnTimerPause;
    public UnityEvent<StopWatch> OnTimerResume;
    public UnityEvent<StopWatch> OnTick;
    public float MaxTimerTime { get; private set; }
    public float TotalRunningTime { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsPaused { get; private set; }

    public void Initialize(float maxTimerTim)
    {
        MaxTimerTime = maxTimerTim;
        TotalRunningTime = 0f;
        IsRunning = false;
        IsPaused = false;
    }

    public void StartTimer()
    {
        IsRunning = true;
        IsPaused = false;
        TotalRunningTime = 0f;
        OnTimerStart.Invoke(this);
    }

    public void PauseTimer()
    {
        IsPaused = true;
        OnTimerPause?.Invoke(this);
    }

    public void ResumeTimer()
    {
        if (!IsPaused)
        {
            return;
        }

        IsPaused = false;
        OnTimerResume?.Invoke(this);
    }

    public void StopTimer()
    {
        IsRunning = false;
        IsPaused = false;
        OnTimerStop.Invoke(this);
    }

    void Update()
    {
        if (!IsRunning)
        {
            return;
        }

        if (IsPaused)
        {
            return;
        }

#if DEVELOPMENT
        if (Input.GetKeyDown(KeyCode.P)) { DebugStopTimer(); }
#endif
        TotalRunningTime += Time.deltaTime;
        OnTick.Invoke(this);
        if (TotalRunningTime >= MaxTimerTime)
        {
            StopTimer();
        }
    }

    void Start()
    {
        Initialize(180f);
        StartTimer();
    }

    public void DebugStopTimer()
    {
        TotalRunningTime = MaxTimerTime < 5 ? TotalRunningTime
                                            : MaxTimerTime - 5f;
    }
}
