using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Sound
{
    public class SoundManagerComponent : MonoBehaviour
    {
        private static SoundManagerComponent _instance;

        public static SoundManagerComponent Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                _instance = new GameObject().AddComponent<SoundManagerComponent>();
                DontDestroyOnLoad(_instance.gameObject);
                _MusicChannel = _instance.AddComponent<AudioSource>();
                _MusicChannel.loop = true;
                _SoundList = Resources.Load<SoundList>("SoundList");
                _instance.Awake();

                return _instance;
            }
        }

        private static SoundList _SoundList;
        private static AudioSource _MusicChannel;

        public int _InitSoundChannelCount = 10;
        private List<AudioSource> _SoundChannels;

        private void Awake()
        {
            _SoundChannels = new List<AudioSource>();

            for (var i = 0; i < _InitSoundChannelCount; i++)
            {
                _SoundChannels.Add(gameObject.AddComponent<AudioSource>());
            }
        }

        #region Sound

        public float PlaySound(string clipName)
        {
            if (_SoundList.AudioClips.TryGetValue(clipName, out var clip))
            {
                var channelIndex = GetEmptyChannelIndex(_SoundChannels);

                _SoundChannels[channelIndex].clip = clip;
                _SoundChannels[channelIndex].Play();
                return _SoundChannels[channelIndex].clip.length;
            }
            else
            {
                Debug.LogError("audio clip " + clipName + " does not exist");
                return 0f;
            }
        }

        private int GetEmptyChannelIndex(IList<AudioSource> soundChannels)
        {
            for (var i = 0; i < soundChannels.Count; i++)
            {
                if (soundChannels[i].isPlaying == false)
                    return i;
            }

            soundChannels.Add(this.gameObject.AddComponent<AudioSource>());
            return soundChannels.Count - 1;
        }

        public void StopSound()
        {
            for (var i = 0; i < _SoundChannels.Count; i++)
            {
                _SoundChannels[i].Stop();
            }
        }

        public void MuteSound()
        {
            for (var i = 0; i < _SoundChannels.Count; i++)
            {
                _SoundChannels[i].volume = 0f;
            }
        }

        public void ListenSound()
        {
            for (var i = 0; i < _SoundChannels.Count; i++)
            {
                _SoundChannels[i].volume = 1f;
            }
        }

        #endregion

        #region Music

        public float PlayMusic(string clipName)
        {
            if (_MusicChannel.clip != null && _MusicChannel.isPlaying && _MusicChannel.clip.name.Equals(clipName))
            {
                return 0f;
            }

            if (_SoundList.AudioClips.TryGetValue(clipName, out AudioClip clip))
            {
                StopMusic();
                _MusicChannel.clip = clip;
                _MusicChannel.Play();
                return _MusicChannel.clip.length;
            }
            else
            {
                Debug.LogError("audio clip " + clipName + " does not exist");
                return 0f;
            }
        }

        public void StopMusic()
        {
            _MusicChannel.Stop();
        }

        public void MuteMusic()
        {
            _MusicChannel.volume = 0f;
        }

        public void ListenMusic()
        {
            _MusicChannel.volume = 1f;
        }

        public void SetMusicVolum(float volume, float duration)
        {
            StartCoroutine(LerpVolume(_MusicChannel, volume, duration));
        }

        private static IEnumerator LerpVolume(AudioSource audioChannel, float targetVolume, float duration)
        {
            var beginVolume = audioChannel.volume;
            var volumeOffset = targetVolume - beginVolume;
            for (float time = 0; time < duration; time += Time.deltaTime)
            {
                audioChannel.volume = beginVolume + (volumeOffset * (time / duration));
                yield return null;
            }

            audioChannel.volume = targetVolume;
        }

        #endregion

        public void MuteAll()
        {
            MuteMusic();
            MuteSound();
        }

        public void ListenAll()
        {
            ListenMusic();
            ListenSound();
        }
    }
}