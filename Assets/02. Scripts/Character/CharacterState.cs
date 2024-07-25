using System;
using UnityEngine;

namespace PlatformGame.Character
{
    [Flags, Serializable]
    public enum CharacterStateFlags
    {
        None = 0,
        Idle = 1 << 1,
        Move = 1 << 2,
        Jump = 1 << 3,
        Fall = 1 << 4,
        Action = 1 << 5,
        Rest = 1 << 6
    }

    [Serializable]
    public enum CharacterState
    {
        Idle,
        Walk,
        Running,
        Jumping,
        Falling,
        Land,
        Attack,
        Attack2,
        AttackDelay,
        Interaction,
        Die,
        Rest,
        ReleaseRest,
        Pray,
        Watering
    }

    public static class StateCheck
    {
        public static bool Equals(CharacterState state, CharacterStateFlags flags)
        {
            switch (state)
            {
                case CharacterState.Idle: return (flags & CharacterStateFlags.Idle) == CharacterStateFlags.Idle;
                case CharacterState.Walk: return (flags & CharacterStateFlags.Move) == CharacterStateFlags.Move;
                case CharacterState.Running: return (flags & CharacterStateFlags.Move) == CharacterStateFlags.Move;
                case CharacterState.Jumping: return (flags & CharacterStateFlags.Jump) == CharacterStateFlags.Jump;
                case CharacterState.Falling: return (flags & CharacterStateFlags.Fall) == CharacterStateFlags.Fall;
                case CharacterState.Attack: return (flags & CharacterStateFlags.Action) == CharacterStateFlags.Action;
                case CharacterState.Attack2: return (flags & CharacterStateFlags.Action) == CharacterStateFlags.Action;
                case CharacterState.AttackDelay: return (flags & CharacterStateFlags.Action) == CharacterStateFlags.Action;
                case CharacterState.Land: return (flags & CharacterStateFlags.Jump) == CharacterStateFlags.Jump;
                case CharacterState.Interaction: return (flags & CharacterStateFlags.Action) == CharacterStateFlags.Action;
                case CharacterState.Rest: return (flags & CharacterStateFlags.Rest) == CharacterStateFlags.Rest;
                case CharacterState.ReleaseRest: return (flags & CharacterStateFlags.Rest) == CharacterStateFlags.Rest;
                case CharacterState.Pray: return (flags & CharacterStateFlags.Action) == CharacterStateFlags.Action;
                case CharacterState.Watering: return (flags & CharacterStateFlags.Action) == CharacterStateFlags.Action;

                default: Debug.Assert(false, $"Undefined values : {state}"); return false;
            }
        }
    }

}