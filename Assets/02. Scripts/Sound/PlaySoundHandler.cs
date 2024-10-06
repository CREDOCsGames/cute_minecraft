using UnityEngine;

public class PlaySoundHandler : MonoBehaviour
{
    public void PlaySound(string soundName)
    {
        SoundManagerComponent.Instance.PlaySound(soundName);
    }
    public void PlayMusic(string soundName)
    {
        SoundManagerComponent.Instance.PlayMusic(soundName);
    }
}
