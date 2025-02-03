using Flow;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Controller
{
    public sealed class PortalComponent : MonoBehaviour
    {
        public WorkState State { get; private set; }
        private readonly List<Transform> _entrylist = new();
        [Range(0, byte.MaxValue)]
        [SerializeField] private byte _needCharacters = 1;
        [SerializeField] private LayerMask CharacterTag;
        [SerializeField] private UnityEvent _runEvent;
        [SerializeField] private UnityEvent _endEvent;

        private void RunPortal()
        {
            State = WorkState.Action;
            _runEvent.Invoke();
        }

        private bool CanRunningPortal(Transform other)
        {
            return State is WorkState.Ready &&
                   _needCharacters <= _entrylist.Count();
        }

        private void OnTriggerEnter(Collider other)
        {
            var character = other.transform;
            if (!other.CompareTag("Character") ||
                _entrylist.Contains(character))
            {
                return;
            }

            if (!CanRunningPortal(character))
            {
                return;
            }

            RunPortal();
        }

        private void OnTriggerExit(Collider other)
        {
            var character = other.transform;

            if (_entrylist.Contains(character))
            {
                _entrylist.Remove(character);
            }

            if (State is WorkState.Action &&
                _entrylist.Count() < _needCharacters)
            {
                State = WorkState.Ready;
                _endEvent.Invoke();
            }
        }
    }
}