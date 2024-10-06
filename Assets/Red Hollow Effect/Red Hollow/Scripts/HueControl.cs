using UnityEngine;

public class HueControl : MonoBehaviour
{
    public float hue = 0;

    public HueValue[] hues;

    void Start()
    {
        for (int i = 0; i < hues.Length; i++)
        {
            hues[i].hue = hue;
        }
    }

    void Update()
    {
        for (int i = 0; i < hues.Length; i++)
        {
            hues[i].hue = hue;
        }
    }

}
