using System.Collections.Generic;
using UnityEngine;

namespace Sound
{
    [CreateAssetMenu(fileName = "SoundList", menuName = "ScriptableObject/SoundList", order = int.MaxValue)]
    public class SoundList : ScriptableObject
    {
        public List<AudioClip> _AudioClipList;

        private Dictionary<string, AudioClip> _audioClipDictionary;

        public Dictionary<string, AudioClip> AudioClips
        {
            get
            {
                if (_audioClipDictionary == null || _AudioClipList.Count != _audioClipDictionary.Count)
                    _audioClipDictionary = GetSoundList();

                return _audioClipDictionary;
            }
        }

        private Dictionary<string, AudioClip> GetSoundList()
        {
            var audioDictionary = new Dictionary<string, AudioClip>();

            foreach (var item in _AudioClipList)
            {
                if (!audioDictionary.TryAdd(item.name, item))
                    continue;
            }

            return audioDictionary;
        }
    }
}