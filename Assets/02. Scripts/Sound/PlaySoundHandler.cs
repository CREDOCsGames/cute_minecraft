using UnityEngine;

public class PlaySoundHandler : MonoBehaviour
{
    public void PlaySound(string soundName)
    {
        SoundManager.Instance.PlaySound(soundName);
    }
    public void PlayMusic(string soundName)
    {
        SoundManager.Instance.PlayMusic(soundName);
    }
}
