using PlatformGame.Contents.Loader;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Contents
{
    public class PortalComponent : MonoBehaviour
    {
        [Range(0, byte.MaxValue)]
        public byte NeedCharacters = 1;
        protected WorkState State { get; set; }
        protected readonly Dictionary<Character.CharacterComponent, bool> mEntrylist = new();
        [SerializeField] UnityEvent mRunEvent;
        [SerializeField] UnityEvent mEndEvent;

        protected virtual void RunPortal()
        {
            State = WorkState.Action;
            mRunEvent.Invoke();
        }

        protected virtual bool CanRunningPortal(Character.CharacterComponent other)
        {
            Debug.Assert(NeedCharacters <= mEntrylist.Count);
            return State == WorkState.Ready &&
                   NeedCharacters <= mEntrylist.Count(x => x.Value);
        }

        public void ResetEntrylist()
        {
            mEntrylist.Clear();
            var characters = PlayerCharacterManager.Instance.JoinCharacters;
            foreach (var character in characters)
            {
                mEntrylist.Add(character, false);
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
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

        protected virtual void OnTriggerExit(Collider other)
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