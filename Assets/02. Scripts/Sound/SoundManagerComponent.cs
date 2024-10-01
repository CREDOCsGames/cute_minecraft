using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Sound
{
    public class SoundManagerComponent : MonoBehaviour
    {
        private static SoundManagerComponent m_instance;

        public static SoundManagerComponent Instance
        {
            get
            {
                if (m_instance != null)
                {
                    return m_instance;
                }

                m_instance = new GameObject().AddComponent<SoundManagerComponent>();
                DontDestroyOnLoad(m_instance.gameObject);
                m_MusicChannel = m_instance.AddComponent<AudioSource>();
                m_MusicChannel.loop = true;
                m_SoundList = Resources.Load<SoundList>("SoundList");
                m_instance.Awake();

                return m_instance;
            }
        }

        static SoundList m_SoundList;
        static AudioSource m_MusicChannel;

        public int m_InitSoundChannelCount = 10;
        private List<AudioSource> m_SoundChannels;

        void Awake()
        {
            m_SoundChannels = new List<AudioSource>();

            for (var i = 0; i < m_InitSoundChannelCount; i++)
            {
                m_SoundChannels.Add(gameObject.AddComponent<AudioSource>());
            }
        }

        #region Sound

        public float PlaySound(string clipName)
        {
            if (m_SoundList.AudioClips.TryGetValue(clipName, out var clip))
            {
                var channelIndex = GetEmptyChannelIndex(m_SoundChannels);

                m_SoundChannels[channelIndex].clip = clip;
                m_SoundChannels[channelIndex].Play();
                return m_SoundChannels[channelIndex].clip.length;
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
            for (var i = 0; i < m_SoundChannels.Count; i++)
            {
                m_SoundChannels[i].Stop();
            }
        }

        public void MuteSound()
        {
            for (var i = 0; i < m_SoundChannels.Count; i++)
            {
                m_SoundChannels[i].volume = 0f;
            }
        }

        public void ListenSound()
        {
            for (var i = 0; i < m_SoundChannels.Count; i++)
            {
                m_SoundChannels[i].volume = 1f;
            }
        }

        #endregion

        #region Music

        public float PlayMusic(string clipName)
        {
            if (m_MusicChannel.clip != null && m_MusicChannel.isPlaying && m_MusicChannel.clip.name.Equals(clipName))
            {
                return 0f;
            }

            if (m_SoundList.AudioClips.TryGetValue(clipName, out AudioClip clip))
            {
                StopMusic();
                m_MusicChannel.clip = clip;
                m_MusicChannel.Play();
                return m_MusicChannel.clip.length;
            }
            else
            {
                Debug.LogError("audio clip " + clipName + " does not exist");
                return 0f;
            }
        }

        public void StopMusic()
        {
            m_MusicChannel.Stop();
        }

        public void MuteMusic()
        {
            m_MusicChannel.volume = 0f;
        }

        public void ListenMusic()
        {
            m_MusicChannel.volume = 1f;
        }

        public void SetMusicVolum(float volume, float duration)
        {
            StartCoroutine(LerpVolume(m_MusicChannel, volume, duration));
        }

        static IEnumerator LerpVolume(AudioSource audioChannel, float targetVolume, float duration)
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