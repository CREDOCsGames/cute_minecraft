using Flow;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Character
{
    public sealed class PortalComponent : MonoBehaviour
    {
        [Range(0, byte.MaxValue)] public byte NeedCharacters = 1;

        private WorkState State { get; set; }
        private readonly Dictionary<Character.CharacterComponent, bool> mEntrylist = new();
        [SerializeField] UnityEvent mRunEvent;
        [SerializeField] UnityEvent mEndEvent;

        void RunPortal()
        {
            State = WorkState.Action;
            mRunEvent.Invoke();
        }

        bool CanRunningPortal(Character.CharacterComponent other)
        {
            Debug.Assert(NeedCharacters <= mEntrylist.Count);
            return State == WorkState.Ready &&
                   NeedCharacters <= mEntrylist.Count(x => x.Value);
        }

        void ResetEntrylist()
        {
            mEntrylist.Clear();
            var characters = PlayerCharacterManager.JoinCharacters;
            foreach (var character in characters)
            {
                mEntrylist.Add(character, false);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            var character = other.GetComponent<Character.CharacterComponent>();
            if (!(character && character.CompareTag(Character.CharacterComponent.TAG_PLAYER)))
            {
                return;
            }

            mEntrylist[character] = true;

            if (!CanRunningPortal(character))
            {
                return;
            }

            RunPortal();
        }

        void OnTriggerExit(Collider other)
        {
            var character = other.GetComponent<Character.CharacterComponent>();
            if (!(character && character.CompareTag(Character.CharacterComponent.TAG_PLAYER)))
            {
                return;
            }

            if (mEntrylist.ContainsKey(character))
            {
                mEntrylist[character] = false;
            }

            if (!(State is WorkState.Action))
            {
                return;
            }

            if (mEntrylist.Any(x => x.Value))
            {
                return;
            }


            State = WorkState.Ready;
            mEndEvent.Invoke();
        }

        void Start()
        {
            ResetEntrylist();
        }
    }
}