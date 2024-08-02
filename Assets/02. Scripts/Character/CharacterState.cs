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
        Die = 1 << 6
    }

    [Serializable]
    public enum CharacterState
    {
        Idle,
        Walk,
        Run,
        Jump,
        Fall,
        Land,
        Action1,
        Action2,
        Die
    }

    public static class StateCheck
    {
        public static bool Equals(CharacterState state, CharacterStateFlags flags)
        {
            switch (state)
            {
                case CharacterState.Idle: return (flags & CharacterStateFlags.Idle) == CharacterStateFlags.Idle;
                case CharacterState.Walk: return (flags & CharacterStateFlags.Move) == CharacterStateFlags.Move;
                case CharacterState.Run: return (flags & CharacterStateFlags.Move) == CharacterStateFlags.Move;
                case CharacterState.Jump: return (flags & CharacterStateFlags.Jump) == CharacterStateFlags.Jump;
                case CharacterState.Fall: return (flags & CharacterStateFlags.Fall) == CharacterStateFlags.Fall;
                case CharacterState.Action1: return (flags & CharacterStateFlags.Action) == CharacterStateFlags.Action;
                case CharacterState.Action2: return (flags & CharacterStateFlags.Action) == CharacterStateFlags.Action;
                case CharacterState.Land: return (flags & CharacterStateFlags.Jump) == CharacterStateFlags.Jump;
                case CharacterState.Die: return (flags & CharacterStateFlags.None) == CharacterStateFlags.None;

                default: Debug.Assert(false, $"Undefined values : {state}"); return false;
            }
        }
    }

}