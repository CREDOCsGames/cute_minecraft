using PlatformGame.Contents.Loader;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Contents
{
    public class Portal : MonoBehaviour
    {
        [Range(0, byte.MaxValue)]
        public byte NeedCharacters = 1;
        protected WorkState State { get; set; }
        protected readonly Dictionary<Character.Character, bool> mEntrylist = new();
        [SerializeField] UnityEvent mRunEvent;
        [SerializeField] UnityEvent mEndEvent;

        protected virtual void RunPortal()
        {
            State = WorkState.Action;
            mRunEvent.Invoke();
        }

        protected virtual bool CanRunningPortal(Character.Character other)
        {
            Debug.Assert(NeedCharacters <= mEntrylist.Count);
            return State == WorkState.Ready &&
                   NeedCharacters <= mEntrylist.Count(x => x.Value);
        }

        void ResetEntrylist()
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
            var character = other.GetComponent<Character.Character>();
            if (!(character && character.CompareTag(Character.Character.TAG_PLAYER)))
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

            var character = other.GetComponent<Character.Character>();
            if (!(character && character.CompareTag(Character.Character.TAG_PLAYER)))
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