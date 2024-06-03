using PlatformGame.Contents.Loader;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Contents
{
    public abstract class Portal : MonoBehaviour
    {
        protected WorkState State { get; set; }
        protected readonly Dictionary<Character.Character, bool> mEntrylist = new();
        [SerializeField] UnityEvent mRunEvent;

        protected virtual void RunPortal()
        {
            State = WorkState.Action;
            mRunEvent.Invoke();
        }

        protected abstract bool CanRunningPortal(Character.Character other);

        void ResetEntrylist()
        {
            mEntrylist.Clear();
            var characters = GameManager.Instance.JoinCharacters;
            foreach (var character in characters)
            {
                mEntrylist.Add(character, false);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            var character = other.GetComponent<Character.Character>();
            if (!character)
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
            var character = other.GetComponent<Character.Character>();
            if (!character)
            {
                return;
            }

            if (mEntrylist.ContainsKey(character))
            {
                mEntrylist[character] = false;
            }

            if (mEntrylist.All(x => x.Value == false))
            {
                State = WorkState.Ready;
            }
        }

        void Start()
        {
            ResetEntrylist();
        }

    }
}