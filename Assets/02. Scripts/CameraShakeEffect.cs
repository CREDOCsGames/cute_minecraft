using UnityEngine;

public class CameraShakeEffect : MonoBehaviour
{
    public float ShakeAmount;
    float shakeTime;

    Vector3 initPos;

    public void VibeTime(float time)
    {
        shakeTime = time;
    }


    private void Start()
    {
        initPos = new Vector3(0f, 0f, -5f);
    }

    private void Update()
    {
        if (shakeTime > 0)
        {
            transform.position = Random.insideUnitSphere * ShakeAmount + initPos;
            shakeTime -= Time.deltaTime;
        }

        else
        {
            shakeTime = 0.0f;
            transform.position = initPos;
        }
    }
}
