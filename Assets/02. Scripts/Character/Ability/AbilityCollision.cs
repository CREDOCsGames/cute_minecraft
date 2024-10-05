using System;
using UnityEngine;

namespace PlatformGame.Character.Combat
{
    [Serializable]
    public struct AbilityCollision
    {
        public readonly Transform Caster;
        public readonly Transform Victim;
        public readonly Ability Ability;

        public AbilityCollision(Transform caster, Transform victim, Ability ability)
        {
            Caster = caster;
            Victim = victim;
            Ability = ability;
        }

    }
}