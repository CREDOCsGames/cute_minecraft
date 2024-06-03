using System;

namespace PlatformGame.Character.Combat
{
    [Serializable]
    public struct AbilityCollision
    {
        public readonly Character Caster;
        public readonly Character Victim;
        public readonly Ability Ability;

        public AbilityCollision(Character caster, Character victim, Ability ability)
        {
            Caster = caster;
            Victim = victim;
            Ability = ability;
        }

    }
}