using Flow;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Character
{
    public sealed class PortalComponent : MonoBehaviour
    {
        [Range(0, byte.MaxValue)]
        [SerializeField] private byte _needCharacters = 1;
        public WorkState State { get; private set; }
        private readonly Dictionary<CharacterComponent, bool> _entrylist = new();
        [SerializeField] private UnityEvent _runEvent;
        [SerializeField] private UnityEvent _endEvent;

        private void RunPortal()
        {
            State = WorkState.Action;
            _runEvent.Invoke();
        }

        private bool CanRunningPortal(Character.CharacterComponent other)
        {
            Debug.Assert(_needCharacters <= _entrylist.Count);
            return State == WorkState.Ready &&
                   _needCharacters <= _entrylist.Count(x => x.Value);
        }

        private void ResetEntrylist()
        {
            _entrylist.Clear();
            var characters = PlayerCharacterManager.JoinCharacters;
            foreach (var character in characters)
            {
                _entrylist.Add(character, false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var character = other.GetComponent<Character.CharacterComponent>();
            if (!(character && character.CompareTag(Character.CharacterComponent.TAG_PLAYER)))
            {
                return;
            }

            _entrylist[character] = true;

            if (!CanRunningPortal(character))
            {
                return;
            }

            RunPortal();
        }

        private void OnTriggerExit(Collider other)
        {
            var character = other.GetComponent<Character.CharacterComponent>();
            if (!(character && character.CompareTag(Character.CharacterComponent.TAG_PLAYER)))
            {
                return;
            }

            if (_entrylist.ContainsKey(character))
            {
                _entrylist[character] = false;
            }

            if (!(State is WorkState.Action))
            {
                return;
            }

            if (_entrylist.Any(x => x.Value))
            {
                return;
            }


            State = WorkState.Ready;
            _endEvent.Invoke();
        }

        private void Start()
        {
            ResetEntrylist();
        }
    }
}