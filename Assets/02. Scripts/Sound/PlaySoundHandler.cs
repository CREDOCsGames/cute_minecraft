using System.Collections;
using UnityEngine;

public class PlaySoundHandler : MonoBehaviour
{
    public string m_StartSound = "";

    public void OnEnable()
    {
        if (0 < m_StartSound.Length)
            StartCoroutine(DelayPlaySound(0.1f));
    }

    private IEnumerator DelayPlaySound(float v)
    {
        yield return new WaitForSeconds(v);
        PlaySound(m_StartSound);
    }

    public void PlaySound(string soundName)
    {
        SoundManager.Instance.PlaySound(soundName);
    }
}
