using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float mTimeScale;

    void Update()
    {
        Time.timeScale = mTimeScale;
    }

    public void SetScale(float t)
    {
        mTimeScale = t;
        Time.timeScale = mTimeScale;
    }
    void OnDestroy()
    {
        SetScale(1f);
    }
}
