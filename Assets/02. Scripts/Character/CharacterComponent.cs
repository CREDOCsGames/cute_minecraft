using Movement;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Character
{
    public class CharacterComponent : MonoBehaviour
    {
        public const string TAG_PLAYER = "Player";
        private static readonly List<CharacterComponent> _instances = new();
        public static IEnumerable<CharacterComponent> Instances => _instances.ToList();
        public bool IsAction => _cooltime.IsAction;
        public Func<bool> IsGrounded { get; set; } = () => true;
        private CharacterState _state;

        public CharacterState State
        {
            get => _state;
            private set
            {
                if (_state != value)
                {
                    OnChangedState.Invoke(value);
                }

                _state = value;
            }
        }

        [SerializeField] private Rigidbody _rigid;
        public Rigidbody Rigid => _rigid;
        [SerializeField] private Animator _animator;
        public Animator Animator => _animator;

        [SerializeField] private MovementComponent _movement;
        private MovementComponent Movement => _movement;
        private readonly Cooltime _cooltime = new();

        [Header("Options")][SerializeField] private ActionDataList _hasAbilities;
        public ActionDataList HasAbilities => _hasAbilities;
        [SerializeField] private UnityEvent<CharacterState> _onChangedState;
        public UnityEvent<CharacterState> OnChangedState => _onChangedState;

        public void DoAction(ActionData data)
        {
            DoAction(data.ID);
        }

        public void DoAction(int actionID)
        {
            _hasAbilities.Library.TryGetValue(actionID, out var action);
            Debug.Assert(action, $"The {actionID} is not registered as an ability for {gameObject.name}.");

            if (!StateCheck.Equals(State, action.AllowedState))
            {
                return;
            }

            State = action.BeState;

            _cooltime.UseAbility(action);

            if (!action.Movement)
            {
                return;
            }

            Movement.PlayMovement(action.Movement);
        }

        public void ResetAction()
        {
            _cooltime.Reset();
        }

        private void Awake()
        {
            Debug.Assert(Rigid, $"Rigidbody reference not found : {gameObject.name}");
            Debug.Assert(_movement, $"Movement reference not found : {gameObject.name}");
            _instances.Add(this);
        }

        private void Start()
        {
            OnChangedState.Invoke(State);
        }

        private void OnDestroy()
        {
            _instances.Remove(this);
        }
    }
}