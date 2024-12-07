using System;
using UnityEngine;

namespace Character
{
    public class StateBaseAnimatorHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private CharacterComponent _character;

        [SerializeField] Animator _animator;
        private string[] _triggers;

        private void Awake()
        {
            Debug.Assert(_character != null, $"Not found Character : {name}");
            _character.OnChangedState.AddListener(SendTrigger);
            _triggers = Enum.GetNames(typeof(CharacterState));
        }

        private void SendTrigger(CharacterState state)
        {
            foreach (var trigger in _triggers)
            {
                _animator.ResetTrigger(trigger);
            }

            _animator.SetTrigger(state.ToString());
        }

        private void OnDestroy()
        {
            _character.OnChangedState.RemoveListener(SendTrigger);
        }
    }
}